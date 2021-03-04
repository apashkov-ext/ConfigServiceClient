using System;
using ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;
using ConfigServiceClient.Tests.Fixtures;
using Moq;
using Xunit;

namespace ConfigServiceClient.Tests.UnitTests
{
    public class ConfigLoaderTests
    {
        [Fact]
        public async void TryLoadJsonAsync_EmptyRemoteConfigEmptyCachedConfig_ReturnsNull()
        {
            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var cacheMock = new Mock<IJsonCache>();
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.Zero);
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Null(json);
        }

        [Fact]
        public async void TryLoadJsonAsync_NonEmptyRemoteConfigEmptyCachedConfig_ReturnsRemoteConfig()
        {
            const string expected = "{}";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => expected);

            var cacheMock = new Mock<IJsonCache>();
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.FromHours(1));
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(expected, json);
        }

        [Fact]
        public async void TryLoadJsonAsync_EmptyRemoteConfigNonEmptyCachedConfig_ReturnsCachedConfig()
        {
            const string expected = "{ \"name\":\"Config\" }";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var cacheMock = new Mock<IJsonCache>();
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => new JsonCacheEntry { Content = expected, Modified = DateTime.Now });

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.FromHours(1));
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(expected, json);
        }

        [Fact]
        public async void TryLoadJsonAsync_RemoteConfigThrowsExExpiredCachedConfig_ReturnsExpired()
        {
            const string expected = "{ \"name\":\"Config\" }";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

            var cacheMock = new Mock<IJsonCache>();
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => new JsonCacheEntry { Content = expected, Modified = DateTime.Now });

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.Zero);
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(expected, json);
        }

        [Fact]
        public async void TryLoadJsonAsync_RemoteConfigThrowsExNonExpiredCachedConfig_ReturnsExpired()
        {
            const string expected = "{ \"name\":\"Config\" }";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

            var cacheMock = new Mock<IJsonCache>();
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => new JsonCacheEntry { Content = expected, Modified = DateTime.Now });

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.FromHours(1));
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(expected, json);
        }

        [Fact]
        public async void TryLoadJsonAsync_ExpiredCacheConfig_ReturnsRemoteConfig()
        {
            const string remote = "{ \"name\":\"RemoteConfig\" }";
            const string cached = "{ \"name\":\"CachedConfig\" }";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => remote);

            var cacheMock = new Mock<IJsonCache>();
            var entry = new JsonCacheEntry
            {
                Content = cached,
                Modified = DateTime.Now.Subtract(TimeSpan.FromHours(1))
            };
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => entry);

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.FromSeconds(1));
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(remote, json);
        }

        [Fact]
        public async void TryLoadJsonAsync_NonExpiredCacheConfig_ReturnsCached()
        {
            const string remote = "{ \"name\":\"RemoteConfig\" }";
            const string cached = "{ \"name\":\"CachedConfig\" }";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => remote);

            var cacheMock = new Mock<IJsonCache>();
            var entry = new JsonCacheEntry
            {
                Content = cached,
                Modified = DateTime.Now.Subtract(TimeSpan.FromHours(1))
            };
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => entry);

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.FromHours(2));
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(cached, json);
        }

        [Fact]
        public async void TryLoadJsonAsync_ExpiredCacheConfigThrowsWhilePuttingNewConfig_ReturnsExpired()
        {
            const string remote = "{ \"name\":\"RemoteConfig\" }";
            const string cached = "{ \"name\":\"CachedConfig\" }";

            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => remote);

            var cacheMock = new Mock<IJsonCache>();
            var entry = new JsonCacheEntry
            {
                Content = cached,
                Modified = DateTime.Now.Subtract(TimeSpan.FromHours(2))
            };
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => entry);
            cacheMock.Setup(x => x.Put(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.FromHours(1));
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Equal(cached, json);
        }
    }
}
