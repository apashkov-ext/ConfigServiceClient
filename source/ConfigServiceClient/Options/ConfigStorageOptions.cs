using System;

namespace ConfigServiceClient.Options
{
    /// <summary>
    /// Configuration Service Client options.
    /// </summary>
    public class ConfigClientOptions
    {
        /// <summary>
        /// Configuration Service instance web api url.
        /// </summary>
        public string ConfigServiceApiEndpoint { get; set; }

        /// <summary>
        /// Name of the project for which the configuration will be retrieved.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Key to access the project configuration. This key is generated when creating a project using the UI client or web api.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
         /// Cached configuration lifetime. If value equals to TimeSpan.Zero cached config never will be expired.
         /// Default value: TimeSpan.Zero.
         /// </summary>
        public TimeSpan CacheExpiration { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Number of attempts to request a remote configuration via the web api.
        /// Default value: 2.
        /// </summary>
        public int RemoteConfigRequestingAttemptsCount { get; set; } = 2;

        /// <summary>
        /// Timeout of remote configuration requesting attempt.
        /// Default value: 2 sec.
        /// </summary>
        public TimeSpan RemoteConfigRequestingTimeout { get; set; } = TimeSpan.FromSeconds(2);
    }
}