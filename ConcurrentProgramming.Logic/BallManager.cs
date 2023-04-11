using System;
using System.Collections.Generic;
using System.Net;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class BallManager : IBallManager
{
    private readonly IBallRepository _ballRepository;
    private readonly Random _random = new();

    public BallManager(IBallRepository ballRepository)
    {
        _ballRepository = ballRepository;
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
            var ball = new Ball(ballX, ballY, diameter, vel);
            _ballRepository.Add(ball);
            BallCreated?.Invoke(this, new BallEventArgs(ball));
        }
    }

    public void Stop()
    {
        _ballRepository.Dispose();
    }

    public void Dispose()
    {
        _ballRepository.Dispose();
    }
}