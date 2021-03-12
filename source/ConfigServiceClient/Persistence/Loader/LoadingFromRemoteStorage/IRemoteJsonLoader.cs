using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    public interface IRemoteJsonLoader
    {
        Task<string> GetAsync(string uri);
    }
}
