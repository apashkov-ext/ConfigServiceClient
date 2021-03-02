using System;
using System.Threading.Tasks;
using ConfigServiceClient.Api;
using ConfigServiceClient.Persistence;

namespace ConfigServiceClient
{
    public class ConfigServiceClient
    {
        private readonly ConfigStorage _configStorage;

        public ConfigServiceClient(Action<ConfigClientOptions> configure = null)
        {
            var options = new ConfigClientOptions();
            configure?.Invoke(options);

            _configStorage = new ConfigStorage(options);
        }

        protected ConfigServiceClient()
        {

        }

        protected ConfigServiceClient(ConfigStorage configStorage)
        {
            _configStorage = configStorage;
        }

        public async Task<IConfigObject> LoadAsync(string environment)
        {
            var g = await _configStorage.GetConfig(environment);
            return new ConfigObject(g);
        }

        public Task<T> LoadAsync<T>(string environment) where T : class
        {
            return _configStorage.GetConfig<T>(environment);
        }
    }
}