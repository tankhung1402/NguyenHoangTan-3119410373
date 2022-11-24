using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Pay1193.Models
{
    public class PaymentDetailViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string NiNo { get; set; }
        [DataType(DataType.Date),Display(Name ="Date Pay")]
        public DateTime PayDate { get; set; }
        [Display(Name = "Month Pay")]
        public string PayMonth { get; set; }
        public string Year { get; set; }
        public string TaxCode { get; set; }
        public decimal HourlyRate{ get; set; }
        public decimal HourlyWorked { get; set; }
        public decimal ContractualHours { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal ContractualEarnings { get; set; }
        public decimal OvertimeEarnings { get; set; }
        public decimal Tax { get; set; }
        public decimal NiC { get; set; }
        public decimal UnionFee { get; set; }
        public Nullable<decimal> SLC { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }

    }
}
