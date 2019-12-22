using System.Net.Http;
using System.Threading.Tasks;
using WorkerService.Core.Interfaces;

namespace WorkerService.Infrastructure
{
    public class HttpService : IHttpService
    {
        public async Task<int> GetUrlResponseStatusCodeAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);

                return (int)result.StatusCode;
            }
        }
    }
}
