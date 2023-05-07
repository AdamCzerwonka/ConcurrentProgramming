using System;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class Ball : IBall
{
    private readonly Timer _timer;
    private Vec2 _velocity;
    private readonly int _width;
    private readonly int _height;

    public Ball(int x, int y, int diameter, Vec2 velocity, int width, int height)
    {
        _position = new Vec2(x, y);
        _velocity = velocity;
        _width = width;
        _height = height;
        Diameter = diameter;
        _timer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(16));
    }

    public Vec2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    private Vec2 _position;

    public int X => _position.X;

    public int Y => _position.Y;
    public int Diameter { get; }
    public event EventHandler<BallEventArgs>? BallChanged;

    private void Move(object? _)
    {        
        _position += _velocity;
        
        if (_position.X - Diameter / 2 <= 0 || _position.X + Diameter / 2>= _width)
        {
            _velocity.X = -_velocity.X;
        }
        
        if (_position.Y - Diameter / 2 <= 0 || _position.Y + Diameter / 2>= _height)
        {
            _velocity.Y = -_velocity.Y;
        }
        
        BallChanged?.Invoke(this, new BallEventArgs(this));
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}