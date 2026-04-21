using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public interface ITenderService
    {
        Task<CompleteTenderInfo> GetCompleteTenderInfoAsync(string tenderId);
    }
}
