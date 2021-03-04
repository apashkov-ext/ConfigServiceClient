using ConfigServiceClient.Persistence.Storage;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigServiceClient : ConfigServiceClient
    {
        public TestableConfigServiceClient(ConfigStorage storage) : base(storage) { }
    }
}
