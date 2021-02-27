using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConfigServiceClient.Abstractions;
using ConfigServiceClient.Api;
using ConfigServiceClient.ConfigLoading;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient
{
    public class ConfigServiceClient
    {
        private readonly DefaultHttpClient _httpClient;
        private readonly string _project;

        public ConfigServiceClient(string configServiceApiEndpoint, string project, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(configServiceApiEndpoint))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(configServiceApiEndpoint));
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(apiKey));
            }

            if (string.IsNullOrWhiteSpace(project))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(project));
            }

            var http = new HttpClient
            {
                BaseAddress = new Uri(configServiceApiEndpoint),
                DefaultRequestHeaders =
                {
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue("application/json"),
                    },
                    UserAgent =
                    {
                        new ProductInfoHeaderValue("ConfigurationServiceClient", GetType().Assembly.GetName().Version?.ToString())
                    }
                }
            };

            http.DefaultRequestHeaders.Add("ApiKey", apiKey);

            _httpClient = new DefaultHttpClient(http);
            _project = project;
        }

        public IConfigObject Load(string environment)
        {
            var loadTask = LoadAsync(environment);
            var task = Task.Run(async () => await loadTask);
            task.Wait();

            return task.Result;
        }

        public async Task<IConfigObject> LoadAsync(string environment)
        {
            var g = await GetOptionGroup(environment);
            return new ConfigObject(g);
        }

        public T Load<T>(string environment) where T : class
        {
            var loadTask = LoadAsync<T>(environment);
            var task = Task.Run(async () => await loadTask);
            task.Wait();

            return task.Result;
        }

        public Task<T> LoadAsync<T>(string environment) where T : class
        {
            return GetConfig<T>(environment);
        }

        private async Task<T> GetConfig<T>(string environment) where T : class
        {
            return await _httpClient.GetAsync<T>($"api/projects/{_project}/configs/{environment}");
        }

        private async Task<OptionGroup> GetOptionGroup(string environment)
        {
            return await _httpClient.GetAsync<OptionGroup>($"api/projects/{_project}/option-groups/{environment}");
        }
    }
}