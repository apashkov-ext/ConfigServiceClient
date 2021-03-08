using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    public static class HttpClientFactory
    {
        public static IHttpClient GetHttpClient(ConfigClientOptions options, string appVersion)
        {
            if (string.IsNullOrWhiteSpace(options.ConfigServiceApiEndpoint))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(options.ConfigServiceApiEndpoint));
            }

            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(options.ApiKey));
            }

            return new DefaultHttpClient(GetHttp(options, appVersion));
        }

        private static HttpClient GetHttp(ConfigClientOptions options, string appVersion)
        {
            var http = new HttpClient(new HttpMessageHandler(new HttpClientHandler(), options.RemoteConfigRequestingAttemptsCount, options.RemoteConfigRequestingTimeout))
            {
                BaseAddress = new Uri(options.ConfigServiceApiEndpoint),
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

            http.DefaultRequestHeaders.Add("ApiKey", options.ApiKey);

            return http;
        }
    }
}
