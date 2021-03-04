using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence.Loader
{
    public interface IConfigLoader
    {
        Task<string> TryLoadJsonAsync(string environment);
    }
}