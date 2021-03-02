using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence.LoadingFromRemoteStorage
{
    public interface IHttpClient
    {
        Task<string> GetAsync(string uri);
    }
}
