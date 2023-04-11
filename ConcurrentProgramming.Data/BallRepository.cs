using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConcurrentProgramming.Data;

public class BallRepository : IBallRepository
{
    private readonly List<IBall> _balls = new();

    public IEnumerable<IBall> Get()
    {
        return new ReadOnlyCollection<IBall>(_balls);
    }

    public void Add(IBall ball)
    {
        _balls.Add(ball);
    }

    public void Remove(IBall ball)
    {
        _balls.Remove(ball);
    }

    public void Dispose()
    {
        foreach (var ball in _balls)
        {
            ball.Dispose();
        }

        _balls.Clear();
    }
}