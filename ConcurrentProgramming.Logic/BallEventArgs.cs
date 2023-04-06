using System;

namespace ConcurrentProgramming.Logic;

public class BallEventArgs : EventArgs
{
    public BallEventArgs(IBall ball)
    {
        Ball = ball;
    }

    public IBall Ball;
}