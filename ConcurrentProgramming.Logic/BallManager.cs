using System;
using System.Collections.Generic;
using System.Net;

namespace ConcurrentProgramming.Logic;

public class BallManager
{
    public BallManager(int amountOfBalls)
    {
        _amountOfBalls = amountOfBalls;
    }

    private int _amountOfBalls;
    private readonly Random _random = new Random();

    public void Start()
    {
        for (var i = 0; i < _amountOfBalls; i++)
        {
            var vel = new Vec2(generateRandom(-5, 5), generateRandom(-5, 5));
            var ball = new Ball(_random.Next(0, 600 - 20), _random.Next(0, 600 - 20), vel);
            BallCreated.Invoke(this, new BallEventArgs { Ball = ball });
        }
    }

    private int generateRandom(int min, int max)
    {
        int num = 0;
        while (num == 0)
        {
            num = _random.Next(min, max);
        }

        return num;
    }


    public event EventHandler<BallEventArgs> BallCreated;
}

public class BallEventArgs : EventArgs
{
    public IBall Ball;
}