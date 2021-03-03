using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigServiceClient.Persistence.LoadingFromRemoteStorage;
using ConfigServiceClient.Persistence.LocalCaching;
using ConfigServiceClient.Tests.Fixtures;
using Moq;
using Xunit;

namespace ConfigServiceClient.Tests.UnitTests
{
    public class ConfigStorageTests
    {
        [Fact]
        public async void LoadAsync_EmptyRemoteConfigEmptyCachedConfig_ThrowsError()
        {
            var clientMock = new Mock<IHttpClient>();
            clientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var cacheMock = new Mock<IJsonCache>();
            cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(() => null);

            var loader = new TestableConfigLoader(clientMock.Object, cacheMock.Object, TimeSpan.Zero);
            var json = await loader.TryLoadJsonAsync("env");

            Assert.Null(json);
        }
    }
}
