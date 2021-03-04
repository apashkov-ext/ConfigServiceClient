using System;
using System.Net.Http;
using System.Threading.Tasks;
using ConfigServiceClient.Persistence.Serialization;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    public class DefaultHttpClient : IHttpClient
    {
        private readonly HttpClient _client;

        public DefaultHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetAsync(string uri)
        {
            var resp = await ExecuteHttpMethod(() => _client.GetAsync(uri));
            return await resp.Content.ReadAsStringAsync();
        }

        private static async Task<T> FromJsonContent<T>(HttpContent content)
        {
            var jsonResponse = await content.ReadAsStringAsync();
            return JsonDeserializer.Deserialize<T>(jsonResponse);
        }

        private static async Task<HttpResponseMessage> ExecuteHttpMethod(Func<Task<HttpResponseMessage>> method)
        {
            var response = await method();

            try
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException)
            {
                var content = await TryGetContent(response);
                var message = $"An error occurred while accessing the source {(string.IsNullOrEmpty(content) ? string.Empty : $": {content}")}";

                switch ((int) response.StatusCode) 
                {
                    case 404:
                        return null;
                    default: 
                        throw new ApplicationException(message);
                };
            }
        }

        private static async Task<string> TryGetContent(HttpResponseMessage response)
        {
            if (response.Content.Headers.ContentType?.MediaType != "application/json")
            {
                return await response.Content.ReadAsStringAsync();
            }

            var parsed = await FromJsonContent<ErrorObject>(response.Content);
            return parsed.Message;
        }

        private class ErrorObject
        {
            public string Message { get; }
        }
    }
}
