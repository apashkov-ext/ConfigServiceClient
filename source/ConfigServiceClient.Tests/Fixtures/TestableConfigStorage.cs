using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigStorage : ConfigStorage
    {
        public TestableConfigStorage(IConfigLoader loader, IJsonImporter<IOptionGroup> importer) : base(loader, importer) {}
    }
}
