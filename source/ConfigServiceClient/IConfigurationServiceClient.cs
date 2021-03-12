using System.Threading.Tasks;
using ConfigServiceClient.Api;

namespace ConfigServiceClient
{
    public interface IConfigurationServiceClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        /// <example>
        /// <code>
        /// </code>
        /// </example>
        /// <exception cref="ConfigServiceClient.Core.Exceptions.ConfigNotFoundException"></exception>
        /// <returns></returns>
        Task<IConfigObject> LoadAsync(string environment);

        Task<T> LoadAsync<T>(string environment) where T : class;
    }
}