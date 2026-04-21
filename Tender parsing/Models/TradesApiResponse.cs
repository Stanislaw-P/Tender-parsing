namespace Tender_parsing.Models
{
    // Ответ от API при поиске
    public class TradesApiResponse
    {
        public int totalpages { get; set; }
        public int currpage { get; set; }
        public int totalrecords { get; set; }
        public List<TenderBasicInfo> invdata { get; set; }
    }
}
