using System.Threading.Tasks;

namespace ConfigServiceClient.Persistence
{
    public interface IConfigLoader
    {
        Task<string> TryLoadJsonAsync(string environment);
    }
}