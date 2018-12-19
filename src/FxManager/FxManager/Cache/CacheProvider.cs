using System;
using Enyim.Caching;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FxManager.Cache
{
    public interface ICacheProvider
    {
        bool Contains(string key);
        Task<T> TryGet<T>(string key, Func<Task<T>> getRates);
    }

    public class CacheProvider : ICacheProvider
    {
        private readonly IMemcachedClient _memcachedClient;
        private int _cacheTimeout;

        public CacheProvider(IMemcachedClient memcachedClient, IConfiguration configuration)
        {
            _memcachedClient = memcachedClient;
            _cacheTimeout = configuration.GetValue<int>("CacheTimeout");
        }

        public bool Contains(string key)
        {
            return false;
        }

        public async Task<T> TryGet<T>(string key, Func<Task<T>> getRates)
        {
            var currencyRate = await _memcachedClient
                .GetValueOrCreateAsync(key, _cacheTimeout, getRates);
            return currencyRate;
        }
    }
}