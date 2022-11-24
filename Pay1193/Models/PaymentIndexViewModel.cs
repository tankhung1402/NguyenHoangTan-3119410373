using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Pay1193.Models
{
    public class PaymentIndexViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        [Display(Name ="Pay Date")]
        public DateTime DatePay { get; set; }
        [Display(Name = "Pay Month")]
        public string MonthPay { get; set; }
        public int TaxYearId { get; set; }
        public string Year { get; set; }
        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }
        [Display(Name = "Total Deduction")]
        public decimal TotalDeduction { get; set; }
        [Display(Name = "Net Pay")]
        public decimal NetPay { get; set; }

    }
}
