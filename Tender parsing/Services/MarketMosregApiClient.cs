using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public class MarketMosregApiClient : IMarketMosregApiClient
    {
        readonly IHttpClientFactory _httpClientFactory;

        public MarketMosregApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        /// <summary>
        /// Получить базовую информацию о тендере
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        public async Task<string> GetBasicTenderInfoAsync(string tenderId)
        {
            var httpClient = _httpClientFactory.CreateClient("MarketMosregApi");

            var response = await httpClient.PostAsJsonAsync(
                "api/Trade/GetTradesForParticipantOrAnonymous",
                new { page = 1, itemsPerPage = 10, Id = tenderId }
            );
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Получить Получить html страницу извещения тендера
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        public async Task<string> GetTradePageHtmlAsync(string tenderId)
        {
            var httpClient = _httpClientFactory.CreateClient("MarketMosregWeb");

            var response = await httpClient.GetAsync($"https://market.mosreg.ru/Trade/ViewTrade/{tenderId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Получить сипсок документов тендера
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        public async Task<List<TenderDocument>> GetTenderDocumentsAsync(string tenderId)
        {
            var httpClient = _httpClientFactory.CreateClient("MarketMosregApi");
            var response = await httpClient.GetAsync($"api/Trade/{tenderId}/GetTradeDocuments");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<TenderDocument>>() ?? new List<TenderDocument>();
        }
    }
}
