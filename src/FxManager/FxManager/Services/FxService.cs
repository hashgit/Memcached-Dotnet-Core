using System;
using FxManager.Models;

namespace FxManager.Services
{
    public interface IFxService
    {
        FxResponse GetRate(string baseCurrency, string targetCurrency);
    }

    public class FxService : IFxService
    {
        private readonly ISystemConfiguration _systemConfiguration;

        public FxService(ISystemConfiguration systemConfiguration)
        {
            _systemConfiguration = systemConfiguration;
        }

        public FxResponse GetRate(string bCurrency, string tCurrency)
        {
            var baseCurrency = bCurrency.ToUpper();
            var targetCurrency = tCurrency.ToUpper();

            if (!_systemConfiguration.IsCurrencySupported(baseCurrency))
                throw new ArgumentOutOfRangeException(baseCurrency, "Unsupported currency");
            if (!_systemConfiguration.IsCurrencySupported(targetCurrency))
                throw new ArgumentOutOfRangeException(targetCurrency, "Unsupported currency");



            return new FxResponse {BaseCurrency = baseCurrency, TargetCurrency = targetCurrency};
        }
    }
}