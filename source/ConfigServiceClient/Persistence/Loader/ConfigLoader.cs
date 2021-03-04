using System;
using System.Threading.Tasks;
using ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;

namespace ConfigServiceClient.Persistence.Loader
{
    public class ConfigLoader : IConfigLoader
    {
        private readonly string _project;
        private readonly TimeSpan _expiration;
        private readonly IHttpClient _httpClient;
        private readonly IJsonCache _jsonCache;

        public ConfigLoader(ConfigClientOptions options)
        {
            _project = options.Project;
            _expiration = options.CacheExpiration;
            _httpClient = HttpClientFactory.GetHttpClient(options.ConfigServiceApiEndpoint, options.ApiKey, GetType().Assembly.GetName().Version?.ToString());
            _jsonCache = new JsonCache(options.Project);
        }

        protected ConfigLoader(IHttpClient httpClient, IJsonCache jsonCache, TimeSpan expiration)
        {
            _httpClient = httpClient;
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
            var content = await _httpClient.GetAsync(endpoint);
            if (content == null)
            {
                return null;
            }

            _jsonCache.Put(key, content);
            return content;
        }

        private static bool Expired(DateTime modified, TimeSpan expiration)
        {
            return DateTime.Now - modified > expiration;
        }
    }
}
