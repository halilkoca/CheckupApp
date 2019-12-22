using Data.Models;
using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IUrlStatusChecker
    {
        Task CheckUrlAsync(CheckApp checkApp, string requestId);
    }
}
