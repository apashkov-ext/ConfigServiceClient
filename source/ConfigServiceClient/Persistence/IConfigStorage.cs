using System.Threading.Tasks;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Persistence
{
    public interface IConfigStorage
    {
        Task<IOptionGroup> GetConfig(string environment);
        Task<T> GetConfig<T>(string environment);
    }
}