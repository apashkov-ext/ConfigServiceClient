using System.Text.Json;
using System.Threading.Tasks;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Persistence
{
    public class ConfigStorage : IConfigStorage
    {
        private const string DoesNotExistErr = "Config does not exist";
        private readonly IConfigLoader _loader;

        public ConfigStorage(ConfigClientOptions options)
        {
            _loader = new ConfigLoader(options);
        }

        protected ConfigStorage(IConfigLoader loader)
        {
            _loader = loader;
        }

        public async Task<IOptionGroup> GetConfig(string environment)
        {
            var json = await _loader.TryLoadJsonAsync(environment) ?? throw ConfigNotFoundException.Create(DoesNotExistErr);
            var doc = JsonDocument.Parse(json);
            var imported = new OptionGroupHierarchyImporter().ImportFromJson(doc);

            return imported;
        }

        public async Task<T> GetConfig<T>(string environment)
        {
            var json = await _loader.TryLoadJsonAsync(environment) ?? throw ConfigNotFoundException.Create(DoesNotExistErr);
            return JsonDeserializer.Deserialize<T>(json);
        }
    }
}
