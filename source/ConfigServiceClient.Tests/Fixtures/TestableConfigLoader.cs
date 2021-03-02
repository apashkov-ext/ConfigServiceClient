using System;
using ConfigServiceClient.Persistence;
using ConfigServiceClient.Persistence.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigLoader : ConfigLoader
    {
        public TestableConfigLoader(IHttpClient http, IJsonCache cache, TimeSpan expiration) : base(http, cache, expiration) {}
    }
}
