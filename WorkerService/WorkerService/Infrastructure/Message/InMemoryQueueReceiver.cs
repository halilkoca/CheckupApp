using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerService.Core.Interfaces;

namespace WorkerService.Infrastructure.Message
{
    public class InMemoryQueueReceiver : IQueueReceiver
    {
        public static Queue<CheckApp> MessageQueue = new Queue<CheckApp>();

        public async Task<CheckApp> GetMessageFromQueue()
        {
            if (MessageQueue.Count == 0) return null;
            return MessageQueue.Dequeue();
        }
    }
}
