﻿using System;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data.Logger.LogWriter;

public interface ILogWriter : IDisposable
{ 
    Task Write(LogEntry log);
}