using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using ConcurrentProgramming.Logic;

namespace ConcurrentProgramming.Presentation.Model;

public sealed class Ball : INotifyPropertyChanged
{
    private int _left;
    private int _top;

    public int Radius { get; init; }
    public int Diameter => Radius * 2;

    public Color Color { get; init; } = Color.Red;

    public int Left
    {
        get => _left;
        set
        {
            if (value == _left) return;
            _left = value;
            OnPropertyChanged();
        }
    }

    public int Top
    {
        get => _top;
        set
        {
            if (value == _top) return;
            _top = value;
            OnPropertyChanged();
        }
    }

    public void OnBallChanged(object? sender, BallEventArgs e)
    {
        Top = e.Ball.Y;
        Left = e.Ball.X;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}