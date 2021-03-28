using System;
using ConfigServiceClient;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = ConfigurationServiceClient.Create(x =>
            {
                x.ApiKey = "22a71687-4249-4a20-8353-02fa6cd70187";
                x.ConfigServiceApiEndpoint = new Uri("http://localhost:5000");
                x.Project = "mars";
                x.CacheExpiration = TimeSpan.FromMinutes(5);
                x.RemoteConfigRequestingTimeout = TimeSpan.FromSeconds(2);
                x.RemoteConfigRequestingAttemptsCount = 2;
            });

            var task = client.LoadAsync("dev");
            task.Wait();
            var cfg = task.Result;
            var section = cfg.GetNestedObject("logging");
            var value = cfg.GetValue<bool>("logging.loggingEnabled");
        }
    }
}
