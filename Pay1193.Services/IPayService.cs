using Microsoft.AspNetCore.Antiforgery.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Pay1193.Services
{
    public interface IPayService
    {
        Task CreateAsync(PaymentRecord paymentRecord);
        PaymentRecord GetById(int id);
        TaxYear GetTaxYearById(int id);
        Task UpdateAsync(PaymentRecord pay);
        Task DeleteAsync(int id);
        IEnumerable<PaymentRecord> GetAll();
        IEnumerable<SelectListItem> GetAllTaxYear();
        decimal OverTimeHours(decimal hoursWorked, decimal contractualHours);//done
        decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate);
        decimal OverTimerate(decimal hourlyRate);//done
        decimal OverTimeEarnings(decimal overtimeRate, decimal overtimeHours);
        decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings); //done
        decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal UnionFees); //done
        decimal NetPay(decimal totalEarnings, decimal totalDeduction);
    }
}
