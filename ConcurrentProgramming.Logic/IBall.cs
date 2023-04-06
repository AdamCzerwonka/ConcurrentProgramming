using System;

namespace ConcurrentProgramming.Logic;

public interface IBall : IDisposable
{
    int X { get; }
    int Y { get; }
    int Diameter { get; }
    event EventHandler<BallEventArgs> BallChanged;
}