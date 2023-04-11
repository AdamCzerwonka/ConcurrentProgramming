using ConcurrentProgramming.Data;

namespace ConcurrentProgramming.Logic.Tests;

public class BallManagerTests
{
    [Fact]
    public void TestStart_CreatesBallsInRepo_WhenGivenCorrectParams()
    {
        IBallRepository ballRepository = new BallRepository();

        var sut = new BallManager(ballRepository);

        sut.Start(100, 100, 10);

        Assert.Equal(10, ballRepository.Get().Count());
    }
}