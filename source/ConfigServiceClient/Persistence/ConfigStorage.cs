using System.Text.Json;
using System.Threading.Tasks;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Persistence
{
    public class ConfigStorage : IConfigStorage
    {
        private readonly ConfigLoader _loader;

        public ConfigStorage(ConfigClientOptions options)
        {
            _loader = new ConfigLoader(options);
        }

        public async Task<IOptionGroup> GetConfig(string environment)
        {
            var json = await _loader.TryLoadJson(environment) ?? throw ConfigNotFoundException.Create($"Config does not exist");
            var doc = JsonDocument.Parse(json);
            var imported = new OptionGroupHierarchyImporter().ImportFromJson(doc);

            return imported;
        }

        public async Task<T> GetConfig<T>(string environment)
        {
            var json = await _loader.TryLoadJson(environment) ?? throw ConfigNotFoundException.Create($"Config does not exist");
            return JsonDeserializer.Deserialize<T>(json);
        }
    }
}
