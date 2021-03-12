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
            services.AddSingleton<IJsonCache, JsonCache>();
            services.AddSingleton<IConfigLoader, ConfigLoader>();
            services.AddSingleton<IJsonImporter<IOptionGroup>, JsonImporter>();
            services.AddSingleton<IConfigStorage, ConfigStorage>();
            services.AddSingleton<IConfigurationServiceClient, ConfigurationServiceClient>();

            return services;
        }

        private static ConfigClientOptions ApplyOptions(Action<ConfigClientOptions> configure)
        {
            var options = new ConfigClientOptions();
            configure?.Invoke(options);
            return options;
        }
    }
}
