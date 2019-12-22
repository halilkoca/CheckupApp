using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkerService.Core.Interfaces;
using WorkerService.Core.Services;

namespace WorkerService.Infrastructure.Message
{
    public static class ServiceCollectionSetup
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<DataContext>(
                options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );

        public static void AddRepositories(this IServiceCollection services) =>
            services.AddScoped<IRepository, EfRepository>();

        public static void AddMessageQueues(this IServiceCollection services)
        {
            services.AddSingleton<IQueueReceiver, InMemoryQueueReceiver>();
            services.AddSingleton<IQueueSender, InMemoryQueueSender>();
        }

        public static void AddUrlCheckingServices(this IServiceCollection services)
        {
            services.AddTransient<IUrlStatusChecker, UrlStatusChecker>();
            services.AddTransient<IHttpService, HttpService>();
        }
    }
}
