namespace Tender_parsing.Models
{
    public class LotItem
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
