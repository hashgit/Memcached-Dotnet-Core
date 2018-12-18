﻿using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace FxManager.Services
{
    public interface ISystemConfiguration
    {
        IEnumerable<string> GetCurrencies();
        bool IsCurrencySupported(string currency);
    }

    public class SystemConfiguration : ISystemConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly HashSet<string> _supportedCurrencies = new HashSet<string>();

        public SystemConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            BuildSupportedCurrenciesMap();
        }

        private void BuildSupportedCurrenciesMap()
        {
            var currencies = _configuration.GetSection("Currencies").Get<IEnumerable<string>>();
            foreach (var currency in currencies)
            {
                _supportedCurrencies.Add(currency);
            }
        }

        public IEnumerable<string> GetCurrencies()
        {
            return _supportedCurrencies;
        }

        public bool IsCurrencySupported(string currency)
        {
            return _supportedCurrencies.Contains(currency);
        }
    }
}