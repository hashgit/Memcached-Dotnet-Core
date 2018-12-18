using System;

namespace FxManager.Models
{
    public class FxResponse
    {
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime Timestamp { get; set; }
    }
}