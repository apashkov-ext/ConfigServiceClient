using System;
using System.Threading.Tasks;
using ConfigServiceClient.Abstractions;
using ConfigServiceClient.Api;
using ConfigServiceClient.ConfigLoading;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient
{
    public class ConfigServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _project;

        public ConfigServiceClient(string configServiceApiEndpoint, string project, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(project))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(project));
            }

            _httpClient = HttpClientFactory.GetHttpClient(configServiceApiEndpoint, apiKey, GetType().Assembly.GetName().Version?.ToString());
            _project = project;
        }

        protected ConfigServiceClient(string project, IHttpClient httpClient)
        {
            _httpClient = httpClient;
            _project = project;
        }

        public async Task<IConfigObject> LoadAsync(string environment)
        {
            var g = await GetOptionGroup(environment);
            return new ConfigObject(g);
        }

        public Task<T> LoadAsync<T>(string environment) where T : class
        {
            return GetConfig<T>(environment);
        }

        private async Task<T> GetConfig<T>(string environment) where T : class
        {
            if (string.IsNullOrWhiteSpace(environment))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(environment));
            }
            return await _httpClient.GetAsync<T>($"api/projects/{_project}/configs/{environment}") ?? throw GetNotFoundEx(_project, environment);
        }

        private async Task<OptionGroup> GetOptionGroup(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(environment));
            }
            return await _httpClient.GetAsync<OptionGroup>($"api/projects/{_project}/option-groups/{environment}") ?? throw GetNotFoundEx(_project, environment);
        }

        private static ConfigNotFoundException GetNotFoundEx(string proj, string env)
        {
            return ConfigNotFoundException.Create($"Config \"{proj}.{env}\" does not exist");
        }
    }
}