using System;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Options;
using ConfigServiceClient.Persistence.Import;
using ConfigServiceClient.Persistence.Loader;
using ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;
using ConfigServiceClient.Persistence.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigServiceClient
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Configuraton service client.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">A delegate that it used to configure a Configurtion service client.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationServiceClient(this IServiceCollection services, Action<ConfigClientOptions> configure = null)
        {
            return services.AddClient(configure);
        }

        internal static IServiceCollection AddClient(this IServiceCollection services, Action<ConfigClientOptions> configure = null)
        {
            var options = ApplyOptions(configure);
            services.AddSingleton(options);

            services.AddSingleton<IHttpClient>(x => new DefaultHttpClient(options, new HttpMessageHandlerWithPolicy(options)));
            services.AddSingleton<IRemoteJsonLoader, RemoteJsonLoader>();
            services.AddSingleton<ICache<string>, JsonCache>();
            services.AddSingleton<IConfigLoader, ConfigLoader>();
            services.AddSingleton<IJsonParser<IOptionGroup>, OptionGroupJsonParser>();
            services.AddSingleton<IConfigStorage, ConfigStorage>();
            services.AddSingleton<IConfigurationServiceClient>(provider => new ConfigurationServiceClient(provider.GetRequiredService<IConfigStorage>()));

            return services;
        }

        private static ConfigClientOptions ApplyOptions(Action<ConfigClientOptions> configure)
        {
            var options = new ConfigClientOptions();
            configure?.Invoke(options);
            ValidateOptions(options);
            return options;
        }

        private static void ValidateOptions(ConfigClientOptions options)
        {
            if (options.ConfigServiceApiEndpoint == null)
            {
                throw new ApplicationException($"Invalid value for {nameof(ConfigClientOptions.ConfigServiceApiEndpoint)} option.");
            }

            if (string.IsNullOrWhiteSpace(options.Project))
            {
                throw new ApplicationException($"Invalid value for {nameof(ConfigClientOptions.Project)} option.");
            }

            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                throw new ApplicationException($"Invalid value for {nameof(ConfigClientOptions.ApiKey)} option.");
            }

            if (options.RemoteConfigRequestingAttemptsCount <= 0)
            {
                throw new ApplicationException($"Value of the {nameof(ConfigClientOptions.RemoteConfigRequestingAttemptsCount)} option should be more than 0.");
            }

            if (options.RemoteConfigRequestingTimeout == TimeSpan.Zero)
            {
                throw new ApplicationException($"Value of the {nameof(ConfigClientOptions.RemoteConfigRequestingTimeout)} option should be more than TimeSpan.Zero.");
            }
        }
    }
}
