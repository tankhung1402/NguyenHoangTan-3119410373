using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Pay1193.Models
{
    public class PaymentEditViewModel
    {
        public int Id { get; set; }
       
        [Required, StringLength(50), Display(Name = " NI NO.")]
        [RegularExpression(@"[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D\s]$")]
        public string NationalInsuranceNo { get; set; }
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime DatePay { get; set; } = DateTime.Now;
        [Display(Name = "Pay Month")]
        public string MonthPay { get; set; } = DateTime.Today.Month.ToString();     
        public int TaxYearId { get; set; }      
        [Required(ErrorMessage = "Tax Code is required")]
        public string TaxCode { get; set; }
        [Required(ErrorMessage = "Hourly Rate is required")]
        public decimal HourlyRate { get; set; }
        [Required(ErrorMessage = "Hourly Worked is required")]
        public decimal HourlyWorked { get; set; }
        [Required(ErrorMessage = "Contractual Hours is required")]
        public decimal ContractualHours { get; set; }
    }
}
