using System.Text.Json;
using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public class MarketMosregApiClient : IMarketMosregApiClient
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<MarketMosregApiClient> _logger;

        public MarketMosregApiClient(IHttpClientFactory httpClientFactory, ILogger<MarketMosregApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TenderBasicInfo> GetBasicTenderInfoAsync(string tenderId)
        {
            try
            {
                var requestBody = new
                {
                    page = 1,
                    itemsPerPage = 10,
                    Id = tenderId
                };

                var httpClient = _httpClientFactory.CreateClient("MarketMosregApi");

                var response = await httpClient.PostAsJsonAsync(
                    "api/Trade/GetTradesForParticipantOrAnonymous",
                    requestBody);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<TradesApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return apiResponse?.invdata?.FirstOrDefault() ?? new TenderBasicInfo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении базовой информации для тендера {TenderId}", tenderId);
                throw;
            }
        }

        public async Task<string> GetTradePageHtmlAsync(string tenderId)
        {
            var httpClient = _httpClientFactory.CreateClient("MarketMosregWeb");

            var response = await httpClient.GetAsync($"https://market.mosreg.ru/Trade/ViewTrade/{tenderId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<TenderDocument>> GetTenderDocumentsAsync(string tenderId)
        {
            var httpClient = _httpClientFactory.CreateClient("MarketMosregApi");
            var response = await httpClient.GetAsync($"api/Trade/{tenderId}/GetTradeDocuments");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<TenderDocument>>() ?? new List<TenderDocument>();
        }
    }
}
