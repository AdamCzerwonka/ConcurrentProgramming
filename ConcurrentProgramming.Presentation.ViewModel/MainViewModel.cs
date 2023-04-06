using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ConcurrentProgramming.Logic;
using Ball = ConcurrentProgramming.Presentation.Model.Ball;

namespace ConcurrentProgramming.Presentation.ViewModel;

public class MainViewModel : NotifyPropertyChanged
{
    private int _amountOfBalls = 10;

    public MainViewModel()
    {
        StartCommand = new RelayCommand(Start);
    }

    public ICommand StartCommand { get; }

    public int AmountOfBalls
    {
        get => _amountOfBalls;
        set
        {
            _amountOfBalls = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Ball> Balls { get; set; } = new();

    private void Start()
    {
        var manager = new BallManager(AmountOfBalls);
        manager.BallCreated += OnBallCreated;
        manager.Start();
    }

    private void OnBallCreated(object? sender, BallEventArgs e)
    {
        var random = new Random();
        Color[] colors = { Color.Red, Color.Green, Color.Pink, Color.Aqua, Color.Purple, Color.Brown, Color.Orange };
        var ball = new Ball()
        {
            Left = e.Ball.X,
            Top = e.Ball.Y,
            Radius = 10,
            Color = colors[random.Next(colors.Length)]
        };
        e.Ball.BallChanged += ball.OnBallChanged;
        Balls.Add(ball);
    }
}