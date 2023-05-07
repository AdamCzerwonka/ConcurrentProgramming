using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class BallManager : IBallManager
{
    private readonly IBallRepository _ballRepository;
    private readonly Random _random = new();
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentDictionary<IBall, IBall> _collisions;
    private bool _disableCollisions = true;

    public BallManager(IBallRepository ballRepository)
    {
        _ballRepository = ballRepository;
        _semaphore = new SemaphoreSlim(1);
        _collisions = new ConcurrentDictionary<IBall, IBall>();
    }

    public event EventHandler<BallEventArgs>? BallCreated;

    public void Start(int width, int height, int amountOfBalls)
    {
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
            var ball = new Ball(ballX, ballY, diameter, vel, width, height);
            ball.BallChanged += CollisionDetection;
            _ballRepository.Add(ball);
            BallCreated?.Invoke(this, new BallEventArgs(ball));
        }

        _disableCollisions = false;
    }

    public void Stop()
    {
        _ballRepository.Dispose();
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
        _semaphore.Wait();
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

            var xDiff = origin.X - ball.X;
            var yDiff = origin.Y - ball.Y;
            var dist = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

            if (dist <= (origin.Diameter + ball.Diameter) / 2.0)
            {
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

        _semaphore.Release();
    }

    private Vector2 CalculateNewVelocity(IBall ball1, IBall ball2)
    {
        var ball1Vel = ball1.Velocity;
        var ball2Vel = ball2.Velocity;
        var ball1Pos = new Vector2(ball1.X, ball1.Y);
        var ball2Pos = new Vector2(ball2.X, ball2.Y);
        var posDiff = ball1Pos - ball2Pos;
        return ball1.Velocity -
               2.0f * ball2.Mass / (ball1.Mass + ball2.Mass)
               * (Vector2.Dot(ball1Vel - ball2Vel, posDiff) * posDiff) /
               (float)Math.Pow(posDiff.Length(), 2);
    }

    public void Dispose()
    {
        _ballRepository.Dispose();
    }
}