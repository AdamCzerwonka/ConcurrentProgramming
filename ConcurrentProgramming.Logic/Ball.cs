using System;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class Ball : IBall
{
    private readonly Timer _timer;
    private readonly Vec2 _velocity;

    public Ball(int x, int y, int diameter, Vec2 velocity)
    {
        _position = new Vec2(x, y);
        _velocity = velocity;
        Diameter = diameter;
        _timer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(16));
    }

    private Vec2 _position;

    public int X => _position.X;

    public int Y => _position.Y;
    public int Diameter { get; }
    public event EventHandler<BallEventArgs>? BallChanged;

    private void Move(object? _)
    {
        _position += _velocity;

        BallChanged?.Invoke(this, new BallEventArgs(this));
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}