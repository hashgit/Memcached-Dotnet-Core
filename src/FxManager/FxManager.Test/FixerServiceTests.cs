using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FxManager.Cache;
using FxManager.Services;
using FxManager.Services.Http;
using Moq;
using Xunit;

namespace FxManager.Test
{
    public class FixerServiceTests
    {
        [Fact]
        public async Task WillThrowExceptionIfCurrencyNotFound()
        {
            var cacheProvider = new Mock<ICacheProvider>();
            var http = new Mock<IFixerHttp>();

            var rates = new FixerRate
            {
                Rates = new Dictionary<string, decimal>
                {
                    { "AUD", 1 },
                }
            };

            cacheProvider.Setup(x => x.TryGet("AUD-USD", It.IsAny<Func<Task<FixerRate>>>())).Returns(Task.FromResult(rates));

            var service = new FixerService(cacheProvider.Object, http.Object);
            await Assert.ThrowsAsync<CurrencyNotFoundException>(() => service.GetRate("AUD", "USD"));
        }

        [Fact]
        public async Task WillReturnRateIfCurrencyFound()
        {
            var cacheProvider = new Mock<ICacheProvider>();
            var http = new Mock<IFixerHttp>();

            var timestamp = DateTime.Now.Truncate(TimeSpan.TicksPerMinute);
            var rates = new FixerRate
            {
                Timestamp = timestamp.ToUnixTimeSeconds(),
                Rates = new Dictionary<string, decimal>
                {
                    { "AUD", 1 },
                    { "USD", 0.73M }
                }
            };

            cacheProvider.Setup(x => x.TryGet("AUD-USD", It.IsAny<Func<Task<FixerRate>>>())).Returns(Task.FromResult(rates));

            var service = new FixerService(cacheProvider.Object, http.Object);
            var response = await service.GetRate("AUD", "USD");

            Assert.Equal(timestamp, response.Timestamp);
            Assert.Equal(1.3698630136986301369863013699M, response.Rate);
        }
    }
}