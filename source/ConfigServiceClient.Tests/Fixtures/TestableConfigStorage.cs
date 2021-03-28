using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence.Import;
using ConfigServiceClient.Persistence.Loader;
using ConfigServiceClient.Persistence.Storage;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigStorage : ConfigStorage
    {
        public TestableConfigStorage(IConfigLoader loader, IJsonParser<IOptionGroup> importer) : base(loader, importer) {}
    }
}
