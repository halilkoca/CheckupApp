using Data.Models;
using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IQueueSender
    {
        Task SendMessageToQueue(CheckApp checkApp);
    }
}
