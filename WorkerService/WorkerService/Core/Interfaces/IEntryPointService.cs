using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IEntryPointService
    {
        Task ExecuteAsync();
    }
}
