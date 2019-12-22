using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkerService.Core.Interfaces;

namespace WorkerService.Infrastructure.Message
{
    public class InMemoryQueueSender : IQueueSender
    {
        public async Task SendMessageToQueue(CheckApp checkApps)
        {

            InMemoryQueueReceiver.MessageQueue.Enqueue(checkApps);
        }
    }
}
