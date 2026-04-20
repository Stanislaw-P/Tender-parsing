using System.ComponentModel.DataAnnotations;

namespace Tender_parsing.Models
{
    public class TenderSearchViewModel
    {
        [Required(ErrorMessage = "Введите номер тендера")]
        [Display(Name = "Номер тендера")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Номер тендера должен содержать только цифры")]
        public string TenderId { get; set; } = null!;
    }
}
