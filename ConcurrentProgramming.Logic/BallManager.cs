using System;
using System.Collections.Generic;
using System.Net;

namespace ConcurrentProgramming.Logic;

public class BallManager
{
    private readonly int _width;
    private readonly int _height;
    private readonly Random _random = new();

    public BallManager(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public event EventHandler<BallEventArgs>? BallCreated;

    public void Start(int amountOfBalls)
    {
        const int diameter = 40;
        for (var i = 0; i < amountOfBalls; i++)
        {
            var vel = new Vec2(GenerateRandom(-5, 5), GenerateRandom(-5, 5));
            var ballX = _random.Next(20, _width - diameter - 20);
            var ballY = _random.Next(20, _height - diameter - 20);
            var ball = new Ball(ballX, ballY, diameter, vel);
            BallCreated?.Invoke(this, new BallEventArgs(ball));
        }
    }

    private int GenerateRandom(int min, int max)
    {
        int num = 0;
        while (num == 0)
        {
            num = _random.Next(min, max);
        }

        return num;
    }
}