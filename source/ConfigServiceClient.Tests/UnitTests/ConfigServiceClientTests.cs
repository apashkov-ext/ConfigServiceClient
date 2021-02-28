using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigServiceClient.Abstractions;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Tests.Fixtures;
using Moq;
using Xunit;

namespace ConfigServiceClient.Tests.UnitTests
{
    public class ConfigServiceClientTests
    {
        [Fact]
        public void LoadAsync_EmptyJson_ThrowsEx()
        {
            var clientMoq = new Mock<IHttpClient>();
            clientMoq.Setup(x => x.GetAsync<OptionGroup>(It.IsAny<string>())).ReturnsAsync(() => null);
            
            var srvCLient = new TestableConfigServiceClient("proj", clientMoq.Object);

            Assert.ThrowsAsync<ConfigNotFoundException>(() => srvCLient.LoadAsync("env"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void LoadAsync_EmptyEnvParam_ThrowsEx(string env)
        {
            var clientMoq = new Mock<IHttpClient>();
            clientMoq.Setup(x => x.GetAsync<OptionGroup>("env")).ReturnsAsync(() => new OptionGroup());

            var srvCLient = new TestableConfigServiceClient("proj", clientMoq.Object);

            Assert.ThrowsAsync<ArgumentException>(() => srvCLient.LoadAsync(env));
        }

        [Fact]
        public void LoadAsyncGeneric_EmptyJson_ThrowsEx()
        {
            var clientMoq = new Mock<IHttpClient>();
            clientMoq.Setup(x => x.GetAsync<OptionGroup>(It.IsAny<string>())).ReturnsAsync(() => null);

            var srvCLient = new TestableConfigServiceClient("proj", clientMoq.Object);

            Assert.ThrowsAsync<ConfigNotFoundException>(() => srvCLient.LoadAsync<object>("env"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void LoadAsyncGeneric_EmptyEnvParam_ThrowsEx(string env)
        {
            var clientMoq = new Mock<IHttpClient>();
            clientMoq.Setup(x => x.GetAsync<OptionGroup>("env")).ReturnsAsync(() => new OptionGroup());

            var srvCLient = new TestableConfigServiceClient("proj", clientMoq.Object);

            Assert.ThrowsAsync<ArgumentException>(() => srvCLient.LoadAsync<object>(env));
        }

        [Fact]
        public async void LoadAsync_ExistedConfig_ReturnsOptionGroup()
        {
            var clientMoq = new Mock<IHttpClient>();
            clientMoq.Setup(x => x.GetAsync<OptionGroup>("env")).ReturnsAsync(() => new OptionGroup());

            var srvCLient = new TestableConfigServiceClient("proj", clientMoq.Object);

            Assert.NotNull(srvCLient.LoadAsync("en"));
        }
    }
}
