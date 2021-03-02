using ConfigServiceClient.Persistence;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigServiceClient : ConfigServiceClient
    {
        public TestableConfigServiceClient(ConfigStorage storage) : base(storage) { }
    }
}
