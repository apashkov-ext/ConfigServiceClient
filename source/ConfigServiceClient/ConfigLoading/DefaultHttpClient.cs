using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConfigServiceClient.Core.Exceptions;

namespace ConfigServiceClient.ConfigLoading
{
    internal class DefaultHttpClient
    {
        private readonly HttpClient _client;

        public DefaultHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var resp = await ExecuteHttpMethod(() => _client.GetAsync(uri));
            return JsonSerializer.Deserialize<T>(await resp.Content.ReadAsStringAsync(), SerializerOptions.JsonSerializerOptions);
        }

        private static async Task<T> FromJsonContent<T>(HttpContent content)
        {
            var jsonResponse = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse, SerializerOptions.JsonSerializerOptions);
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

                throw (int)response.StatusCode switch
                {
                    404 => ConfigNotFoundException.Create(message),
                    _ => new ApplicationException(message)
                };
            }
        }

        private static async Task<string> TryGetContent(HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return null;
            }

            if (response.Content.Headers.ContentType.MediaType != "application/json")
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
