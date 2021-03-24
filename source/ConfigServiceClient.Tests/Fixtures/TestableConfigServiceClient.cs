using ConfigServiceClient.Persistence.Storage;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigServiceClient : ConfigurationServiceClient
    {
        public TestableConfigServiceClient(IConfigStorage storage) : base(storage) { }
    }
}
