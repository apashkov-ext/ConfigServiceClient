using System.Net.Http;
using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}