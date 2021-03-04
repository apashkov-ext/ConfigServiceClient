﻿using System.Threading.Tasks;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Persistence
{
    public class ConfigStorage : IConfigStorage
    {
        private const string DoesNotExistErr = "Config does not exist";
        private readonly IConfigLoader _loader;
        private readonly IJsonImporter<IOptionGroup> _importer;

        public ConfigStorage(ConfigClientOptions options)
        {
            _loader = new ConfigLoader(options);
            _importer = new JsonImporter();
        }

        protected ConfigStorage(IConfigLoader loader, IJsonImporter<IOptionGroup> importer)
        {
            _loader = loader;
            _importer = importer;
        }

        public async Task<T> GetConfigAsync<T>(string environment) where T : class
        {
            var json = await _loader.TryLoadJsonAsync(environment) ?? throw ConfigNotFoundException.Create(DoesNotExistErr);

            if (typeof(T) != typeof(IOptionGroup))
            {
                return JsonDeserializer.Deserialize<T>(json);
            }
;
            var imported = _importer.ImportFromJson(json);

            return (T)imported;
        }
    }
}
