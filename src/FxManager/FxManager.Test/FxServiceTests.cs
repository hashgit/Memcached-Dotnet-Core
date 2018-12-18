using System;
using System.Threading.Tasks;
using FxManager.Services;
using FxManager.Services.Entities;
using Moq;
using Xunit;

namespace FxManager.Test
{
    public class FxServiceTests
    {
        [Fact]
        public async Task WillThrowExceptionForInvalidBaseCurrency()
        {
            var systemConfig = new Mock<ISystemConfiguration>();
            var fixerService = new Mock<IFixerService>();

            systemConfig.Setup(x => x.IsCurrencySupported("AUD")).Returns(false);
            systemConfig.Setup(x => x.IsCurrencySupported("USD")).Returns(true);

            var service = new FxService(systemConfig.Object, fixerService.Object);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetRate("AUD", "USD"));
        }

        [Fact]
        public async Task WillThrowExceptionForInvalidTargetCurrency()
        {
            var systemConfig = new Mock<ISystemConfiguration>();
            var fixerService = new Mock<IFixerService>();

            systemConfig.Setup(x => x.IsCurrencySupported("AUD")).Returns(true);
            systemConfig.Setup(x => x.IsCurrencySupported("USD")).Returns(false);

            var service = new FxService(systemConfig.Object, fixerService.Object);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetRate("AUD", "USD"));
        }

        [Fact]
        public async Task WillThrowExceptionForSameCurrencyPair()
        {
            var systemConfig = new Mock<ISystemConfiguration>();
            var fixerService = new Mock<IFixerService>();

            systemConfig.Setup(x => x.IsCurrencySupported("AUD")).Returns(true);

            var service = new FxService(systemConfig.Object, fixerService.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => service.GetRate("AUD", "AUD"));
        }

        [Fact]
        public async Task WillReturnRateForValidPair()
        {
            var systemConfig = new Mock<ISystemConfiguration>();
            var fixerService = new Mock<IFixerService>();

            systemConfig.Setup(x => x.IsCurrencySupported("AUD")).Returns(true);
            systemConfig.Setup(x => x.IsCurrencySupported("USD")).Returns(true);
            systemConfig.Setup(x => x.Round).Returns(5);

            var timestamp = DateTime.Now;
            fixerService.Setup(x => x.GetRate("AUD", "USD")).Returns(Task.FromResult(new FxRate { Rate = 1.5671827648273M, Timestamp = timestamp }));

            var service = new FxService(systemConfig.Object, fixerService.Object);

            var response = await service.GetRate("AUD", "USD");
            Assert.Equal("AUD", response.BaseCurrency);
            Assert.Equal("USD", response.TargetCurrency);
            Assert.Equal(timestamp, response.Timestamp);
            Assert.Equal(1.56718M, response.ExchangeRate);
        }
    }
}
