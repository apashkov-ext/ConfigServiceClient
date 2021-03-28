using System;
using System.Threading.Tasks;
using ConfigServiceClient.Options;
using ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;

namespace ConfigServiceClient.Persistence.Loader
{
    public class ConfigLoader : IConfigLoader
    {
        private readonly string _project;
        private readonly TimeSpan _expiration;
        private readonly IRemoteJsonLoader _jsonLoader;
        private readonly ICache<string> _jsonCache;

        public ConfigLoader(IRemoteJsonLoader jsonLoader, ICache<string> jsonCache, ConfigClientOptions options)
        {
            _jsonLoader = jsonLoader;
            _jsonCache = jsonCache;
            _project = options.Project;
            _expiration = options.CacheExpiration;
        }

        protected ConfigLoader(IRemoteJsonLoader jsonLoader, ICache<string> jsonCache, TimeSpan expiration)
        {
            _jsonLoader = jsonLoader;
            _jsonCache = jsonCache;
            _expiration = expiration;
        }

        public async Task<string> TryLoadJsonAsync(string environment)
        {
            var uri = $"api/projects/{_project}/configs/{environment}";
            var key = $"{_project}.{environment}";

            var entry = _jsonCache.Get(key);
            if (entry == null)
            {
                return await LoadAndPutToCache(key, uri);
            }

            if (!Expired(entry.Modified, _expiration))
            {
                return entry.Content;
            }

            try
            {
                return await LoadAndPutToCache(key, uri);
            }
            catch
            {
                return entry.Content;
            }
        }

        private async Task<string> LoadAndPutToCache(string key, string endpoint)
        {
            var content = await _jsonLoader.GetAsync(endpoint);
            if (content == null)
            {
                return null;
            }

            _jsonCache.Put(key, content);
            return content;
        }

        private static bool Expired(DateTime modified, TimeSpan expiration)
        {
            if (expiration == TimeSpan.Zero)
            {
                return false;
            }

            return DateTime.Now - modified > expiration;
        }
    }
}
