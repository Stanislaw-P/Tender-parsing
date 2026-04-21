using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public class TenderHtmlParser : ITenderHtmlParser
    {
        readonly ILogger<TenderHtmlParser> _logger;

        public TenderHtmlParser(ILogger<TenderHtmlParser> logger)
        {
            _logger = logger;
        }

        public Task<TenderAdditionalInfo> ParseAdditionalInfoAsync(string html)
        {
            throw new NotImplementedException();
        }
    }
}
