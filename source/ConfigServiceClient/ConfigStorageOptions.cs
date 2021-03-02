using System;

namespace ConfigServiceClient
{
    public class ConfigClientOptions
    {
        public string ConfigServiceApiEndpoint { get; set; }
        public string Project { get; set; }
        public string ApiKey { get; set; }
        public TimeSpan Expiration { get; set; }
    }
}