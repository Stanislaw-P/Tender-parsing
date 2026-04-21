
using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public interface IMarketMosregApiClient
    {
        Task<string> GetBasicTenderInfoAsync(string tenderId);
        Task<List<TenderDocument>> GetTenderDocumentsAsync(string tenderId);
        Task<string> GetTradePageHtmlAsync(string tenderId);
    }
}