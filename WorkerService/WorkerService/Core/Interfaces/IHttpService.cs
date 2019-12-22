using System.Threading.Tasks;

namespace WorkerService.Core.Interfaces
{
    public interface IHttpService
    {
        Task<int> GetUrlResponseStatusCodeAsync(string url);
    }
}
