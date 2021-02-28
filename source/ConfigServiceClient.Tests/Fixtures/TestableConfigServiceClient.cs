using ConfigServiceClient.Abstractions;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigServiceClient : ConfigServiceClient
    {
        public TestableConfigServiceClient(string proj, IHttpClient http) : base(proj, http) { }
    }
}
