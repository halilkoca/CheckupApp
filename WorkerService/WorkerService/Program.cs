using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService.Core.Interfaces;
using WorkerService.Core.Services;
using WorkerService.Core.Settings;
using WorkerService.Infrastructure;
using WorkerService.Infrastructure.Message;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));
                services.AddSingleton<IEntryPointService, EntryPointService>();
                services.AddSingleton<IServiceLocator, ServiceScopeFactoryLocator>();
                services.AddSingleton<IEmailService, EmailService>();

                // Infrastructure.ContainerSetup
                services.AddMessageQueues();
                services.AddDbContext(hostContext.Configuration);
                services.AddRepositories();
                services.AddUrlCheckingServices();

                var workerSettings = new WorkerSettings();
                hostContext.Configuration.Bind(nameof(WorkerSettings), workerSettings);
                services.AddSingleton(workerSettings);

                var entryPointSettings = new EntryPointSettings();
                hostContext.Configuration.Bind(nameof(EntryPointSettings), entryPointSettings);
                services.AddSingleton(entryPointSettings);

                services.AddHostedService<Worker>();
            });
    }
}
