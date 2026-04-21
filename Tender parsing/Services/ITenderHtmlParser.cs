using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public interface ITenderHtmlParser
    {
        Task<TenderAdditionalInfo> ParseAdditionalInfoAsync(string html);
    }
}
