using System;

namespace ConfigServiceClient
{
    public class ConfigClientOptions
    {
        public string ConfigServiceApiEndpoint { get; set; }
        public string Project { get; set; }
        public string ApiKey { get; set; }
        public TimeSpan CacheExpiration { get; set; }
        public int RemoteConfigRetrieveRetryingCount { get; set; }
        public TimeSpan RemoteConfigRetrieveTimeout { get; set; }
    }
}