using System;
using System.Threading.Tasks;
using FxManager.Cache;
using FxManager.Services.Entities;
using FxManager.Services.Http;

namespace FxManager.Services
{
    public interface IFixerService
    {
        Task<FxRate> GetRate(string baseCurrency, string targetCurrency);
    }

    public class FixerService : IFixerService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IFixerHttp _fixerHttp;

        public FixerService(ICacheProvider cacheProvider, IFixerHttp fixerHttp)
        {
            _cacheProvider = cacheProvider;
            _fixerHttp = fixerHttp;
        }

        public async Task<FxRate> GetRate(string baseCurrency, string targetCurrency)
        {
            var key = $"{baseCurrency}-{targetCurrency}";
            var cached = _cacheProvider.TryGet(key);
            if (cached != null)
            {
                return new FxRate { Rate = 1, Timestamp = DateTime.Now};
            }
            else
            {
                var fixerRate = await _fixerHttp.GetRate(baseCurrency, targetCurrency);
                var baseRate = fixerRate.Rates[baseCurrency];
                var targetRate = fixerRate.Rates[targetCurrency];

                var rate = baseRate / targetRate;
                var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var date = start.AddSeconds(fixerRate.Timestamp).ToLocalTime();

                return new FxRate {Rate = rate, Timestamp = date};
            }
        }
    }
}