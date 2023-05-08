using System;
using System.Numerics;
using System.Threading;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public class Ball : IBall
{
    private readonly Timer _timer;
    private Vector2 _velocity;

    public Ball(int x, int y, int diameter, Vector2 velocity, int mass)
    {
        _position = new Vector2(x, y);
        _velocity = velocity;
        Diameter = (int)(0.3 * mass);
        _timer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(32));
        Mass = mass;
        Radius = Diameter / 2;
    }

    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public Vector2 Position => _position;

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


        BallChanged?.Invoke(this, new BallEventArgs(this));
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}