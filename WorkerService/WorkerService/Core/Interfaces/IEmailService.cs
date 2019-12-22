using Data.Models;
using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(CheckApp app, int statusCode, string errorMessage = null);
    }
}
