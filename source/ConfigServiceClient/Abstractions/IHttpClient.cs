using System.Threading.Tasks;

namespace ConfigServiceClient.Abstractions
{
    public interface IHttpClient
    {
        Task<T> GetAsync<T>(string uri);
    }
}
