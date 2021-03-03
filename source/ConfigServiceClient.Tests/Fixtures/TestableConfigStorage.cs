using ConfigServiceClient.Persistence;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigStorage : ConfigStorage
    {
        public TestableConfigStorage(IConfigLoader loader) : base(loader) {}
    }
}
