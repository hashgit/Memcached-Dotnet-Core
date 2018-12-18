using FxManager.Models;

namespace FxManager.Services
{
    public interface IFxService
    {
        FxResponse GetRate(string baseCurrency, string targetCurrency);
    }

    public class FxService : IFxService
    {
        public FxResponse GetRate(string baseCurrency, string targetCurrency)
        {
            return new FxResponse {BaseCurrency = baseCurrency, TargetCurrency = targetCurrency};
        }
    }
}