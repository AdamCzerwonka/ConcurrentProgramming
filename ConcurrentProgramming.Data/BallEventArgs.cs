using System;

namespace ConcurrentProgramming.Data;

public class BallEventArgs : EventArgs
{
    public BallEventArgs(IBall ball)
    {
        Ball = ball;
    }

    public IBall Ball;
}