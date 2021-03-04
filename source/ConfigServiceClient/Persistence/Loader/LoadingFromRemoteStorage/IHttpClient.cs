using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    public interface IHttpClient
    {
        Task<string> GetAsync(string uri);
    }
}
