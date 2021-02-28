using System;
using System.Net.Http;
using System.Net.Http.Headers;
using ConfigServiceClient.Abstractions;

namespace ConfigServiceClient.ConfigLoading
{
    internal static class HttpClientFactory
    {
        public static IHttpClient GetHttpClient(string configServiceApiEndpoint, string apiKey, string appVersion)
        {
            if (string.IsNullOrWhiteSpace(configServiceApiEndpoint))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(configServiceApiEndpoint));
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(apiKey));
            }

            var http = new HttpClient
            {
                BaseAddress = new Uri(configServiceApiEndpoint),
                DefaultRequestHeaders =
                {
                    Accept =
                    {
                        new MediaTypeWithQualityHeaderValue("application/json")
                    },
                    UserAgent =
                    {
                        new ProductInfoHeaderValue("ConfigurationServiceClient", appVersion)
                    }
                }
            };

            http.DefaultRequestHeaders.Add("ApiKey", apiKey);

            return new DefaultHttpClient(http);
        }
    }
}
