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
            var fixerRate = await _cacheProvider.TryGet(key, async () => await _fixerHttp.GetRate(baseCurrency, targetCurrency));

            if (fixerRate == null)
            {
                throw new CurrencyNotFoundException("Rates not available");
            }

            decimal baseRate = 0, targetRate = 0;
            if (!fixerRate.Rates?.TryGetValue(baseCurrency, out baseRate) ?? true)
                throw new CurrencyNotFoundException($"{baseCurrency} not found");

            if (!fixerRate.Rates?.TryGetValue(targetCurrency, out targetRate) ?? true)
                throw new CurrencyNotFoundException($"{targetCurrency} not found");

            var rate = baseRate / targetRate;
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var date = start.AddSeconds(fixerRate.Timestamp).ToLocalTime();

            return new FxRate {Rate = rate, Timestamp = date};
        }
    }
}