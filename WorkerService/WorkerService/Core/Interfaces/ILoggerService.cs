using System;

namespace WorkerService.Core.Interfaces
{
    public interface ILoggerService<T>
    {
        void LogInformation(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
    }
}
