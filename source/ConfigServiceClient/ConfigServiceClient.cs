using System;
using System.Threading.Tasks;
using ConfigServiceClient.Api;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence;
using ConfigServiceClient.Persistence.Storage;

namespace ConfigServiceClient
{
    public class ConfigServiceClient
    {
        private readonly IConfigStorage _storage;

        public ConfigServiceClient(Action<ConfigClientOptions> configure = null)
        {
            var options = new ConfigClientOptions();
            configure?.Invoke(options);

            _storage = new ConfigStorage(options);
        }

        protected ConfigServiceClient(IConfigStorage storage)
        {
            _storage = storage;
        }

        protected ConfigServiceClient(ConfigStorage configStorage)
        {
            _storage = configStorage;
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