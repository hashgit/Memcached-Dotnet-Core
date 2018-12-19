using System;

namespace FxManager.Services
{
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException(string message) : base(message)
        { }
    }
}