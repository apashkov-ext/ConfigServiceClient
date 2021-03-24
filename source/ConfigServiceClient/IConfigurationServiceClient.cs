using System.Threading.Tasks;
using ConfigServiceClient.Api;

namespace ConfigServiceClient
{
    /// <summary>
    /// The client that loads the project configurations.
    /// </summary>
    public interface IConfigurationServiceClient
    {
        /// <summary>
        /// Returns configuration for the specified environment as a hierarchycally structured object.
        /// </summary>
        /// <param name="environment">Environment name.</param>
        /// <example>
        /// <code>
        /// var cfgClient = ConfigurationServiceClient.Create(configure => ...);
        /// var cfg = await cfgClient.LoadAsync("dev");
        /// </code>
        /// </example>
        /// <exception cref="ConfigServiceClient.Core.Exceptions.ConfigNotFoundException"></exception>
        Task<IConfigObject> LoadAsync(string environment);

        /// <summary>
        /// Returns strongly typed configuration for the specified environment as a class hierarchy.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized project configuration.</typeparam>
        /// <param name="environment">Environment name.</param>
        /// <example>
        /// <code>
        /// internal class ProjectConfig {
        /// //...
        /// };
        /// // ...
        /// var cfgClient = ConfigurationServiceClient.Create(configure => ...);
        /// ProjectConfig cfg = await cfgClient.LoadAsync{ProjectConfig}("dev");
        /// </code>
        /// </example>
        /// <exception cref="ConfigServiceClient.Core.Exceptions.ConfigNotFoundException"></exception>
        Task<T> LoadAsync<T>(string environment) where T : class;
    }
}