using System;
using ConfigServiceClient.Persistence.Loader;
using ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableConfigLoader : ConfigLoader
    {
        public TestableConfigLoader(IHttpClient http, IJsonCache cache, TimeSpan expiration) : base(http, cache, expiration) {}
    }
}
