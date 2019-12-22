using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(string appName, string statusCode)
    }
}
