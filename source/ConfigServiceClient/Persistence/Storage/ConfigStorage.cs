using System.Threading.Tasks;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence.Import;
using ConfigServiceClient.Persistence.Loader;
using ConfigServiceClient.Persistence.Serialization;

namespace ConfigServiceClient.Persistence.Storage
{
    public class ConfigStorage : IConfigStorage
    {
        private const string DoesNotExistErr = "Config does not exist";
        private readonly IConfigLoader _loader;
        private readonly IJsonParser<IOptionGroup> _importer;

        public ConfigStorage(IConfigLoader loader, IJsonParser<IOptionGroup> importer)
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
            var imported = _importer.Parse(json);

            return (T)imported;
        }
    }
}
