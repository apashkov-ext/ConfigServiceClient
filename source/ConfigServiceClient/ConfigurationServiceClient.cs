using System;
using System.Threading.Tasks;
using ConfigServiceClient.Api;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Options;
using ConfigServiceClient.Persistence.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigServiceClient
{
    public class ConfigurationServiceClient : IConfigurationServiceClient
    {
        private static ServiceProvider _serviceProvider;
        private readonly IConfigStorage _storage;

        public ConfigurationServiceClient(IConfigStorage storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Creates and returns instance of the configuration service client.
        /// </summary>
        /// <param name="configure">A delegate that is used to configure a client.</param>
        public static IConfigurationServiceClient Create(Action<ConfigClientOptions> configure = null)
        {
            if (_serviceProvider == null)
            {
                var services = new ServiceCollection();
                _serviceProvider = services.AddClient(configure).BuildServiceProvider();
            }
            
            return _serviceProvider.GetRequiredService<IConfigurationServiceClient>();
        }

        public async Task<IConfigObject> LoadAsync(string environment)
        {
            var g = await _storage.GetConfigAsync<IOptionGroup>(environment);
            return new ConfigObject(g);
        }

        public Task<T> LoadAsync<T>(string environment) where T : class
        {
            return _storage.GetConfigAsync<T>(environment);
        }
    }
}