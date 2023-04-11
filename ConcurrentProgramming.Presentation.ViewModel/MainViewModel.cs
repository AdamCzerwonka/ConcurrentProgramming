using System.Collections.ObjectModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ConcurrentProgramming.Data;
using ConcurrentProgramming.Logic;
using Ball = ConcurrentProgramming.Presentation.Model.Ball;

namespace ConcurrentProgramming.Presentation.ViewModel;

public class MainViewModel : NotifyPropertyChanged
{
    private readonly IBallManager _ballManager;
    private int _amountOfBalls = 10;
    private bool _isRunning;

    public MainViewModel(IBallManager ballManager)
    {
        _ballManager = ballManager;
        StartCommand = new RelayCommand(Start, () => !IsRunning);
        RestartCommand = new RelayCommand(Restart, () => IsRunning);
    }

    public ICommand StartCommand { get; }
    public ICommand RestartCommand { get; }

    public int AmountOfBalls
    {
        get => _amountOfBalls;
        set
        {
            _amountOfBalls = value;
            OnPropertyChanged();
        }
    }

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (value == _isRunning) return;
            _isRunning = value;
            (StartCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (RestartCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<Ball> Balls { get; set; } = new();

    private void Restart()
    {
        IsRunning = false;
        _ballManager.Stop();
        Balls.Clear();
    }

    private void Start()
    {
        IsRunning = true;
        _ballManager.BallCreated += OnBallCreated;
        _ballManager.Start(600, 600, AmountOfBalls);
    }

    private void OnBallCreated(object? sender, BallEventArgs e)
    {
        var random = new Random();
        Color[] colors = { Color.Red, Color.Green, Color.Pink, Color.Aqua, Color.Purple, Color.Brown, Color.Orange };
        var ball = new Ball()
        {
            Left = e.Ball.X,
            Top = e.Ball.Y,
            Radius = e.Ball.Diameter / 2,
            Color = colors[random.Next(colors.Length)]
        };
        e.Ball.BallChanged += ball.OnBallChanged;
        Balls.Add(ball);
    }
}