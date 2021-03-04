using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence
{
    public interface IConfigStorage
    {
        Task<T> GetConfigAsync<T>(string environment) where T : class;
    }
}