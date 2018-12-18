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
        public async Task WillReturnCacheResultIfCacheHit()
        {
            var cacheProvider = new Mock<ICacheProvider>();
            var http = new Mock<IFixerHttp>();

            cacheProvider.Setup(x => x.TryGet("AUD-USD")).Returns(new object());

            var service = new FixerService(cacheProvider.Object, http.Object);

            var response = await service.GetRate("AUD", "USD");
            Assert.NotNull(response);
            http.Verify(x => x.GetRate("AUD", "USD"), Times.Never);
        }
    }
}