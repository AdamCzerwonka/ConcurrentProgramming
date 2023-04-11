using ConcurrentProgramming.Data;
using NSubstitute;

namespace ConcurrentProgramming.Logic.Tests;

public class BallManagerTests
{
    [Fact]
    public void TestStart_CreatesBallsInRepo_WhenGivenCorrectParams()
    {
        var counter = 0;
        var ballRepository = Substitute.For<IBallRepository>();
        ballRepository.When(x => x.Add(Arg.Any<IBall>())).Do(x => counter++);
        
        var sut = new BallManager(ballRepository);

        sut.Start(100, 100, 10);

        Assert.Equal(10, counter);
    }
}