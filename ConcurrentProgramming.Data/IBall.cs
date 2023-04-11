using System;

namespace ConcurrentProgramming.Data;

public interface IBall : IDisposable
{
    int X { get; }
    int Y { get; }
    int Diameter { get; }
    event EventHandler<BallEventArgs> BallChanged;
}