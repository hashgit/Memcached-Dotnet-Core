using System;
using System.Threading.Tasks;
using FxManager.Models;

namespace FxManager.Services
{
    public interface IFxService
    {
        Task<FxResponse> GetRate(string baseCurrency, string targetCurrency);
    }

    public class FxService : IFxService
    {
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly IFixerService _fixerService;

        public FxService(ISystemConfiguration systemConfiguration, IFixerService fixerService)
        {
            _systemConfiguration = systemConfiguration;
            _fixerService = fixerService;
        }

        public async Task<FxResponse> GetRate(string bCurrency, string tCurrency)
        {
            var baseCurrency = bCurrency.ToUpper();
            var targetCurrency = tCurrency.ToUpper();

            if (!_systemConfiguration.IsCurrencySupported(baseCurrency))
                throw new ArgumentOutOfRangeException(baseCurrency, "Unsupported currency");
            if (!_systemConfiguration.IsCurrencySupported(targetCurrency))
                throw new ArgumentOutOfRangeException(targetCurrency, "Unsupported currency");

            var rate = await _fixerService.GetRate(baseCurrency, targetCurrency);

            return new FxResponse {BaseCurrency = baseCurrency, TargetCurrency = targetCurrency, ExchangeRate = Math.Round(rate.Rate, _systemConfiguration.Round), Timestamp = rate.Timestamp };
        }
    }
}