using Data.Models;
using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IQueueReceiver
    {
        Task<CheckApp> GetMessageFromQueue();
    }
}
