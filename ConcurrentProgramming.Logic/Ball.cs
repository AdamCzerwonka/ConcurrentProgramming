using System;
using System.Threading;

namespace ConcurrentProgramming.Logic;

public class Ball : IBall
{
    public Ball(int x, int y, Vec2 velocity)
    {
        _position = new Vec2(x, y);
        _velocity = velocity;
        _timer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000 / 60));
    }

    private Vec2 _position;

    public int X => _position.X;

    public int Y => _position.Y;
    public event EventHandler<BallEventArgs>? BallChanged;
    private Timer _timer;

    private readonly Vec2 _velocity;

    private void Move(object? _)
    {
        _position += _velocity;

        BallChanged?.Invoke(this, new BallEventArgs() { Ball = this });
    }
}