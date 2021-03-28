using System.Net.Http;
using System.Net.Http.Headers;
using ConfigServiceClient.Options;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    public class DefaultHttpClient : HttpClient, IHttpClient
    {
        public DefaultHttpClient(ConfigClientOptions options, HttpMessageHandler handler) : base(handler)
        {
            BaseAddress = options.ConfigServiceApiEndpoint;
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ConfigurationServiceClient", "1.0.0"));
            DefaultRequestHeaders.Add("ApiKey", options.ApiKey);
        }
    }
}