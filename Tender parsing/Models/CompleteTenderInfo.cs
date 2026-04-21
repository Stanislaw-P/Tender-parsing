namespace Tender_parsing.Models
{
    public class CompleteTenderInfo
    {
        public TenderBasicInfo BasicInfo { get; set; }
        public TenderAdditionalInfo AdditionalInfo { get; set; }
        public List<TenderDocument> Documents { get; set; } = new List<TenderDocument>();
    }
}
