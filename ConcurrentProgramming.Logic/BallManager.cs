using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class BallManager : IBallManager
{
    private readonly IBallRepository _ballRepository;
    private readonly Random _random = new();
    private readonly SemaphoreSlim _semaphore;
    
    public BallManager(IBallRepository ballRepository)
    {
        _ballRepository = ballRepository;
        _semaphore = new SemaphoreSlim(1);
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

            var vel = new Vec2(velX, velY);
            var ballX = _random.Next(20, width - diameter - 20);
            var ballY = _random.Next(20, height - diameter - 20);
            var ball = new Ball(ballX, ballY, diameter, vel, width, height);
            ball.BallChanged += CollisionDetection;
            _ballRepository.Add(ball);
            BallCreated?.Invoke(this, new BallEventArgs(ball));
        }
    }

    public void Stop()
    {
        _ballRepository.Dispose();
    }

    private void CollisionDetection(object? sender, EventArgs args)
    {
        if (sender is null)
        {
            return;
        }
        
        var origin = (IBall)sender;
        _semaphore.Wait();
        foreach (var ball in _ballRepository.Get())
        {
            if (ball == origin)
            {
                continue;
            }
            
            var xDiff = origin.X - ball.X;
            var yDiff = origin.Y - ball.Y;
            var dist = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

            if (dist <= (origin.Diameter + ball.Diameter) / 2.0)
            {
                origin.Velocity = new Vec2(-ball.Velocity.X, -ball.Velocity.Y);
                ball.Velocity = new Vec2(-origin.Velocity.X, -origin.Velocity.Y);
            }
        }

        _semaphore.Release();
    }

    public void Dispose()
    {
        _ballRepository.Dispose();
    }
}