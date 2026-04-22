namespace Tender_parsing.Models
{
    public class CompleteTenderInfo
    {
        public TenderBasicInfo BasicInfo { get; set; }
        public TenderAdditionalInfo AdditionalInfo { get; set; }
        public List<TenderDocument> Documents { get; set; } = new List<TenderDocument>();

        // Вспомогательные свойства
        public string StatusColorClass
        {
            get
            {
                return BasicInfo?.TradeStateName switch
                {
                    "Прием предложений" => "success",
                    "Завершен" => "secondary",
                    "Отменен" => "danger",
                    _ => "info"
                };
            }
        }

        public TimeSpan TimeRemaining
        {
            get
            {
                var timeLeft = BasicInfo?.FillingApplicationEndDate - DateTime.UtcNow;
                return timeLeft > TimeSpan.Zero ? timeLeft.Value : TimeSpan.Zero;
            }
        }
    }
}
