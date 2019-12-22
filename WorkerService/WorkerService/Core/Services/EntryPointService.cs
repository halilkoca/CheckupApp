using Data;
using System;
using System.Threading.Tasks;
using WorkerService.Core.Interfaces;
using WorkerService.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using Data.Models;

namespace WorkerService.Core.Services
{
    public class EntryPointService : IEntryPointService
    {
        private readonly ILoggerService<EntryPointService> _logger;
        private readonly EntryPointSettings _settings;
        private readonly IQueueReceiver _queueReceiver;
        private readonly IQueueSender _queueSender;
        private readonly IServiceLocator _serviceScopeFactoryLocator;
        private readonly IUrlStatusChecker _urlStatusChecker;

        public EntryPointService(
            ILoggerService<EntryPointService> logger,
            EntryPointSettings settings,
            IQueueReceiver queueReceiver,
            IQueueSender queueSender,
            IServiceLocator serviceScopeFactoryLocator,
            IUrlStatusChecker urlStatusChecker
            )
        {
            _logger = logger;
            _settings = settings;
            _queueReceiver = queueReceiver;
            _queueSender = queueSender;
            _serviceScopeFactoryLocator = serviceScopeFactoryLocator;
            _urlStatusChecker = urlStatusChecker;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("{service} running at: {time}", nameof(EntryPointService), DateTimeOffset.Now);
            try
            {
                // EF Requires a scope so we are creating one per execution here
                using var scope = _serviceScopeFactoryLocator.CreateScope();
                    var repository = scope.ServiceProvider.GetService<IRepository>();
                var apps = repository.GetList<CheckApp>();
                foreach (var item in apps)
                    await _queueSender.SendMessageToQueue(item);

                // queue den okuma
                bool hasItem = true;
                while (hasItem)
                {
                    CheckApp queue = await _queueReceiver.GetMessageFromQueue();
                    if (queue == null)
                    {
                        hasItem = false;
                        return;
                    }

                    // 1 url kontrolü
                    await _urlStatusChecker.CheckUrlAsync(queue, "");
                }
            }
#pragma warning disable CA1031 // genel exception ları yakalama
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EntryPointService)}.{nameof(ExecuteAsync)} threw an exception.");
                //throw;
            }
#pragma warning restore CA1031 // genel exceptionları yakalama
        }
    }
}
