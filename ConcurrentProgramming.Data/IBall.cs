using System;
using System.Numerics;

namespace ConcurrentProgramming.Data;

public interface IBall : IDisposable
{
    int X { get; }
    int Y { get; }
    int Diameter { get; }
    int Mass { get; }
    event EventHandler<BallEventArgs> BallChanged;
    Vector2 Velocity { get; set; }
}