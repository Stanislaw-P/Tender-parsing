
using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public interface IMarketMosregApiClient
    {
        /// <summary>
        /// Получить базовую информацию о тендере
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        Task<TenderBasicInfo> GetBasicTenderInfoAsync(string tenderId);

        /// <summary>
        /// Получить сипсок документов тендера
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        Task<List<TenderDocument>> GetTenderDocumentsAsync(string tenderId);

        /// <summary>
        /// Получить Получить html страницу извещения тендера
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        Task<string> GetTradePageHtmlAsync(string tenderId);
    }
}