using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence.Import;
using ConfigServiceClient.Persistence.Loader;
using ConfigServiceClient.Tests.Fixtures;
using Moq;
using Xunit;

namespace ConfigServiceClient.Tests.UnitTests
{
    public class Config
    {
        public string Name { get; set; }
    }

    public class ConfigStorageTests
    {
        [Fact]
        public async void GetConfigAsync_EmptyRemoteConfigEmptyCachedConfig_ThrowsError()
        {
            var loaderMock = new Mock<IConfigLoader>();
            loaderMock.Setup(x => x.TryLoadJsonAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var importerMock = new Mock<IJsonParser<IOptionGroup>>();
            importerMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(() => null);

            var storage = new TestableConfigStorage(loaderMock.Object, importerMock.Object);

            await Assert.ThrowsAsync<ConfigNotFoundException>(() => storage.GetConfigAsync<object>("env"));
        }

        [Fact]
        public async void GetConfigAsync_GetOfTypeIOptionsGroup_ReturnsIOptionGroupInstance()
        {
            const string config = "{ \"name\":\"Config\" }";

            var loaderMock = new Mock<IConfigLoader>();
            loaderMock.Setup(x => x.TryLoadJsonAsync(It.IsAny<string>())).ReturnsAsync(() => config);

            var importerMock = new Mock<IJsonParser<IOptionGroup>>();
            importerMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(() => new OptionGroup(""));

            var storage = new TestableConfigStorage(loaderMock.Object, importerMock.Object);
            var result = await storage.GetConfigAsync<IOptionGroup>("env");

            Assert.IsAssignableFrom<IOptionGroup>(result);
        }

        [Fact]
        public async void GetConfigAsync_GetOfTypeUserClass_ReturnsUserClassInstance()
        {
            const string config = "{ \"name\":\"Config\" }";

            var loaderMock = new Mock<IConfigLoader>();
            loaderMock.Setup(x => x.TryLoadJsonAsync(It.IsAny<string>())).ReturnsAsync(() => config);

            var importerMock = new Mock<IJsonParser<IOptionGroup>>();
            importerMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(() => null);

            var storage = new TestableConfigStorage(loaderMock.Object, importerMock.Object);
            var result = await storage.GetConfigAsync<Config>("env");

            Assert.IsType<Config>(result);
        }
    }
}
