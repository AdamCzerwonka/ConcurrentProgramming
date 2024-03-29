﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Data.Logger;
using ConcurrentProgramming.Data.Logger.LogWriter;

namespace ConcurrentProgramming.Logic;

public class BallManager : IBallManager
{
    private readonly IBallRepository _ballRepository;
    private readonly Random _random = new();
    private readonly ConcurrentDictionary<IBall, IBall> _collisions;
    private readonly object _collisionLock = new();
    private ILogger _logger;
    private bool _disableCollisions = true;
    private int _height;
    private int _width;

    public BallManager(IBallRepository ballRepository)
    {
        _ballRepository = ballRepository;
        _collisions = new ConcurrentDictionary<IBall, IBall>();
        _logger = Logger.GetLogger();

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        _logger.RegisterWriter(new TextLogWriter(Path.Combine(path, "textLog.txt")));
        _logger.RegisterWriter(new JsonLogWriter(Path.Combine(path, "jsonLog.json")));
        _logger.RegisterWriter(new YamlLogWriter(Path.Combine(path, "yamlLog.yaml")));
        _logger.RegisterWriter(new XmlLogWriter(Path.Combine(path, "xmlLog.xml")));
    }

    public event EventHandler<BallEventArgs>? BallCreated;

    public void Start(int width, int height, int amountOfBalls)
    {
        _logger.Log(LogLevel.Information, "Starting simulation");

        _width = width;
        _height = height;
        const int diameter = 40;
        for (var i = 0; i < amountOfBalls; i++)
        {
            var velX = _random.Next(-5, 5);
            var velY = _random.Next(-5, 5);
            while (velX == 0 & velY == 0)
            {
                velX = _random.Next(-5, 5);
                velY = _random.Next(-5, 5);
            }

            var vel = new Vector2(velX, velY);
            var ballX = _random.Next(20, width - diameter - 20);
            var ballY = _random.Next(20, height - diameter - 20);
            var ballMass = _random.Next(90, 250);
            var ball = new Ball(ballX, ballY, diameter, vel, ballMass);
            ball.BallChanged += WallCollision;
            ball.BallChanged += CollisionDetection;
            _ballRepository.Add(ball);
            BallCreated?.Invoke(this, new BallEventArgs(ball));
        }

        _disableCollisions = false;
    }

    public void Stop()
    {
        _logger.Log(LogLevel.Information, "Simulation Ended");
        _ballRepository.Dispose();
    }

    private void WallCollision(object? sender, EventArgs args)
    {
        if (sender is null)
        {
            return;
        }

        var ball = (IBall)sender;
        var newVel = new Vector2(ball.Velocity.X, ball.Velocity.Y);

        if (ball.Position.X - ball.Radius <= -1)
        {
            newVel.X = Math.Abs(ball.Velocity.X);
            _collisions.Remove(ball, out _);
        }
        else if (ball.Position.X + ball.Radius >= _width)
        {
            newVel.X = -Math.Abs(ball.Velocity.X);
            _collisions.Remove(ball, out _);
        }

        if (ball.Position.Y - ball.Radius <= -1)
        {
            newVel.Y = Math.Abs(ball.Velocity.Y);
            _collisions.Remove(ball, out _);
        }
        else if (ball.Position.Y + ball.Radius >= _height)
        {
            newVel.Y = -Math.Abs(ball.Velocity.Y);
            _collisions.Remove(ball, out _);
        }

        if (ball.Velocity.X != newVel.X || ball.Velocity.Y != newVel.Y)
        {
            _logger.Log(LogLevel.Information,
                $"wall collision at X:{ball.Position.X}, Y:{ball.Position.Y}, R:{ball.Radius}");
        }

        ball.Velocity = newVel;
    }

    private void CollisionDetection(object? sender, EventArgs args)
    {
        if (_disableCollisions)
        {
            return;
        }

        if (sender is null)
        {
            return;
        }

        var origin = (IBall)sender;
        lock (_collisionLock)
        {
            foreach (var ball in _ballRepository.Get())
            {
                if (_collisions.TryGetValue(origin, out var ball1Last) &&
                    _collisions.TryGetValue(ball, out var ball2Last) &&
                    ball1Last == ball && ball2Last == origin)
                {
                    continue;
                }

                if (ball == origin)
                {
                    continue;
                }

                var dist = Vector2.Distance(origin.Position, ball.Position);

                if (dist <= (origin.Diameter + ball.Diameter) / 2.0)
                {
                    _logger.Log(LogLevel.Information,
                        $"ball collision between balls X:{origin.X}, Y:{origin.Y}, R:{origin.Radius} and X:{ball.X}, Y:{ball.Y}, R:{ball.Radius}");
                    var newOriginVel = CalculateNewVelocity(origin, ball);
                    var newBallVel = CalculateNewVelocity(ball, origin);
                    origin.Velocity = newOriginVel;
                    ball.Velocity = newBallVel;

                    _collisions.Remove(origin, out _);
                    _collisions.Remove(ball, out _);
                    _collisions.TryAdd(origin, ball);
                    _collisions.TryAdd(ball, origin);
                }
            }
        }
    }

    private Vector2 CalculateNewVelocity(IBall ball1, IBall ball2)
    {
        var ball1Vel = ball1.Velocity;
        var ball2Vel = ball2.Velocity;
        var posDiff = ball1.Position - ball2.Position;
        return ball1.Velocity -
               2.0f * ball2.Mass / (ball1.Mass + ball2.Mass)
               * (Vector2.Dot(ball1Vel - ball2Vel, posDiff) * posDiff) /
               (float)Math.Pow(posDiff.Length(), 2);
    }

    public void Dispose()
    {
        _logger.Log(LogLevel.Information, "Closing program");
        _logger.Dispose();
        _ballRepository.Dispose();
    }
}