using NSubstitute;

namespace ConcurrentProgramming.Data.Tests;

public class BallRepositoryTests
{
    [Fact]
    public void GetShouldReturnListOfBalls()
    {
        var ballRepository = new BallRepository();

        var ball = Substitute.For<IBall>();
        ballRepository.Add(ball);
        var balls = ballRepository.Get();
        
        Assert.Single(balls);
    }

    [Fact]
    public void CorrectAddRemoveMethods()
    {
        var ballRepository = new BallRepository();

        var ball1 = Substitute.For<IBall>();
        var ball2 = Substitute.For<IBall>();
        var ball3 = Substitute.For<IBall>();
        ballRepository.Add(ball1); 
        ballRepository.Add(ball2); 
        ballRepository.Add(ball3); 
        ballRepository.Remove(ball2); 
        var balls = ballRepository.Get();
        
        Assert.Equal(2, balls.Count());
    }

    [Fact]
    public void DisposeClearRepository()
    {
        var ballRepository = new BallRepository();
        
        var ball1 = Substitute.For<IBall>();
        var ball2 = Substitute.For<IBall>();
        var ball3 = Substitute.For<IBall>();
        ballRepository.Add(ball1); 
        ballRepository.Add(ball2); 
        ballRepository.Add(ball3); 
        
        ballRepository.Dispose();
        var balls = ballRepository.Get();
        
        Assert.Empty(balls);
    }
}