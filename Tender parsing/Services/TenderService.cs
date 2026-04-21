using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public class TenderService : ITenderService
    {
        readonly IMarketMosregApiClient _marketMosregApiClient;
        readonly ITenderHtmlParser _tenderHtmlParser;
        readonly ILogger<TenderService> _logger;

        public TenderService(IMarketMosregApiClient marketMosregApiClient, ITenderHtmlParser tenderHtmlParser, ILogger<TenderService> logger)
        {
            _marketMosregApiClient = marketMosregApiClient;
            _tenderHtmlParser = tenderHtmlParser;
            _logger = logger;
        }

        public async Task<CompleteTenderInfo> GetCompleteTenderInfoAsync(string tenderId)
        {
            try
            {
                // Шаг 1. Получаем базовую информацию о тендере
                var basicInfo = await _marketMosregApiClient.GetBasicTenderInfoAsync(tenderId);

                if (basicInfo == null)
                {
                    _logger.LogWarning("Тендер с номером {TenderId} не найден", tenderId);
                    return new CompleteTenderInfo();
                }

                // Шаг 2: Получаем HTML страницу и парсим дополнительные данные
                var additionalInfo = new TenderAdditionalInfo();

                try
                {
                    string html = await _marketMosregApiClient.GetTradePageHtmlAsync(tenderId);
                    additionalInfo = await _tenderHtmlParser.ParseAdditionalInfoAsync(html);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Не удалось получить дополнительные данные для тендера {TenderId}", tenderId);
                    additionalInfo.DeliveryPlace = "Не удалось загрузить";
                }

                // Шаг 3: Получаем список документов
                var documents = await _marketMosregApiClient.GetTenderDocumentsAsync(tenderId);

                return new CompleteTenderInfo
                {
                    BasicInfo = basicInfo,
                    AdditionalInfo = additionalInfo,
                    Documents = documents
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении полной информации для тендера {TenderId}", tenderId);
                throw;
            }
        }
    }
}
