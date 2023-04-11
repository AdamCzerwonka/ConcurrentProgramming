using System;
using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic;

public interface IBallManager : IDisposable
{
    event EventHandler<BallEventArgs>? BallCreated;
    void Start(int width, int height, int amountOfBalls);

    void Stop();
}