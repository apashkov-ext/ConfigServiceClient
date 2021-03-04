using System;
using ConfigServiceClient;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence;
using ConfigServiceClient.Persistence.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var http = HttpClientFactory.GetHttpClient("http://localhost:5000", "22a71687-4249-4a20-8353-02fa6cd70187", "1.0");
            var cache = new JsonCache("proj");
            var options = new ConfigClientOptions
            {
                ConfigServiceApiEndpoint = "http://localhost:5000",
                ApiKey = "22a71687-4249-4a20-8353-02fa6cd70187",
                CacheExpiration = TimeSpan.FromSeconds(30),
                Project = "mars"
            };
            var storage = new ConfigStorage(options);

            var res = storage.GetConfigAsync<IOptionGroup>("dev");
            res.Wait();
            var cfg = res.Result;

            Console.ReadKey();
        }
    }
}
