namespace Tender_parsing.Models
{
    // Бополнительная информация из HTML
    public class TenderAdditionalInfo
    {
        public string DeliveryPlace { get; set; } = null!;
        public List<LotItem> LotItems { get; set; } = new List<LotItem>();
    }
}
