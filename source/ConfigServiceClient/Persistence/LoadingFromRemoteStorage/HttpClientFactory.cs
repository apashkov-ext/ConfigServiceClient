using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConfigServiceClient.Persistence.LoadingFromRemoteStorage
{
    public static class HttpClientFactory
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

            return new DefaultHttpClient(GetHttp(configServiceApiEndpoint, apiKey, appVersion));
        }

        private static HttpClient GetHttp(string configServiceApiEndpoint, string apiKey, string appVersion)
        {
            var http = new HttpClient(new HttpMessageHandler(new HttpClientHandler(), 3))
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

            return http;
        }
    }
}
