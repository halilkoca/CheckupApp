using Microsoft.Extensions.DependencyInjection;
using System;

namespace WorkerService.Core.Services
{
    public interface IServiceLocator : IDisposable
    {
        IServiceScope CreateScope();
        T Get<T>();
    }
}
