namespace Tender_parsing.Models
{
    public class TenderDocument
    {
        public DateTimeOffset UploadDate { get; set; }
        public string? FileName { get; set; }
        public string? Url { get; set; }
    }
}
