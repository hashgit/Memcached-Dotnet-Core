using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FxManager.Services.Http
{
    public interface IFixerHttp
    {
        Task<FixerRate> GetRate(string baseCurrency, string targetCurrency);
    }

    public class FixerHttp : IFixerHttp
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FixerHttp(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = CreateHttpClient();

            _apiKey = configuration.GetValue<string>("Fixer:ApiKey");
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("Fixer:BaseUrl")),
                Timeout = TimeSpan.FromMilliseconds(_configuration.GetValue<int>("Fixer:Timeout"))
            };

            return httpClient;
        }

        public async Task<FixerRate> GetRate(string baseCurrency, string targetCurrency)
        {
            var url = $"/api/latest?access_key={_apiKey}&symbols={baseCurrency},{targetCurrency}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FixerRate>(content);
            }

            throw new Exception("Could not retrieve rates at this time");
        }
    }
}