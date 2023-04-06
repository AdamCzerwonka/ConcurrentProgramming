using System;

namespace ConcurrentProgramming.Logic;

public interface IBall
{
    int X { get; }
    int Y { get; }
    event EventHandler<BallEventArgs> BallChanged;
}