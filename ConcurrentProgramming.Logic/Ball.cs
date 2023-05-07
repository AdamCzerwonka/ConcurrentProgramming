using System;
using System.Numerics;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class Ball : IBall
{
    private readonly Timer _timer;
    private Vector2 _velocity;
    private readonly int _width;
    private readonly int _height;

    public Ball(int x, int y, int diameter, Vector2 velocity, int width, int height)
    {
        _position = new Vector2(x, y);
        _velocity = velocity;
        _width = width;
        _height = height;
        Diameter = diameter;
        _timer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(16));
        Mass = 1;
        Radius = Diameter / 2;
    }

    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    private Vector2 _position;

    public int X => (int)_position.X;
    public int Y => (int)_position.Y;
    public int Diameter { get; }
    public int Mass { get; }

    public int Radius { get; }
    public event EventHandler<BallEventArgs>? BallChanged;

    private void Move(object? _)
    {
        _position += _velocity;

        if (_position.X - Radius <= 0)
        {
            _velocity.X = Math.Abs(_velocity.X);
        }
        else if (_position.X + Radius >= _width)
        {
            _velocity.X = -Math.Abs(_velocity.X);
        }

        if (_position.Y - Radius <= 0)
        {
            _velocity.Y = Math.Abs(_velocity.Y);
        }
        else if (_position.Y + Radius >= _height)
        {
            _velocity.Y = -Math.Abs(_velocity.Y);
        }

        BallChanged?.Invoke(this, new BallEventArgs(this));
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}