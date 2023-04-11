using System;
using System.Collections.Generic;

namespace ConcurrentProgramming.Data;

public interface IBallRepository : IDisposable
{
    IEnumerable<IBall> Get();
    void Add(IBall ball);
    void Remove(IBall ball);
}