namespace Tender_parsing.Models
{
    public class TenderBasicInfo
    {
        public int Id { get; set; }
        public string TradeName { get; set; } = null!;
        public string TradeStateName { get; set; } = null!;
        public int TradeState { get; set; }
        public string CustomerFullName { get; set; } = null!;
        public decimal InitialPrice { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime FillingApplicationEndDate { get; set; }
        public int OrganizationId { get; set; }
        public int SourcePlatform { get; set; }
        public string SourcePlatformName { get; set; } = null!;
        public bool IsInitialPriceDefined { get; set; }
        public DateTime? LastModificationDate { get; set; }
    }
}
