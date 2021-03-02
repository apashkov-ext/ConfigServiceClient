using System;
using ConfigServiceClient;
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
                Expiration = TimeSpan.FromSeconds(30),
                Project = "mars"
            };
            var storage = new ConfigStorage(options);

            var res = storage.GetConfig("dev");
            res.Wait();
            var cfg = res.Result;

            Console.ReadKey();
        }
    }

    public class Root
    {
        public Validation Validation { get; set; }
        public Logging Logging { get; set; }
        public Test Test { get; set; }
    }

    public class Validation
    {
        public int ValidationLevel { get; set; }
        public bool ValidationEnabled { get; set; }
        public SectionValidation SectionValidation { get; set; }
    }

    public class SectionValidation
    {
        public string[] Sections { get; set; }
        public bool SectionValidatorEnabled { get; set; }
    }

    public class Logging
    {
        public int[] TestArr { get; set; }
        public bool LogInfo { get; set; }
        public bool LogErrors { get; set; }
        public string Prp { get; set; }
        public bool LoggingEnabled { get; set; }
    }

    public class Test
    {
        public Nested Nested { get; set; }
    }

    public class Nested
    {
        public Other Other { get; set; }
    }

    public class Other
    {

    }
}
