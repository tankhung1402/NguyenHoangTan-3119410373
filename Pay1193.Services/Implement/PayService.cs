using Microsoft.AspNetCore.Mvc.Rendering;
using Pay1193.Entity;
using Pay1193.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class PayService : IPayService
    {
        private decimal _OverTimeRate;

        private decimal _OverTimeHours;

        private decimal _ContractualEarnings;

        private decimal _TotalEarnings;

        private decimal _TotalDeduction;

        private readonly ApplicationDbContext _context;

        private readonly ITaxService _taxService;
        public PayService(ApplicationDbContext context, ITaxService taxService)
        {
            _context = context;
            _taxService = taxService;
        }
        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {

            if(hourlyRate < contractualHours)
            {

                _ContractualEarnings = hoursWorked * hourlyRate;

            }
            else
            {

                _ContractualEarnings = contractualHours * hourlyRate;

            }
            return _ContractualEarnings;

        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {

            await _context.PaymentRecords.AddAsync(paymentRecord);
            _context.SaveChanges();

        }

        public IEnumerable<PaymentRecord> GetAll()
        {

            return _context.PaymentRecords.ToList();

        }

        public IEnumerable<SelectListItem> GetAllTaxYear()
        {       
            
            var list = _context.TaxYears.Select(taxYear => new SelectListItem
            {

                Text = taxYear.YearOfTax,
                Value = taxYear.Id.ToString()

            });
            return list;

        }   

        public PaymentRecord GetById(int id)
        {

            return _context.PaymentRecords.Where(p=> p.Id == id).FirstOrDefault();

        }

        public TaxYear GetTaxYearById(int id)
        {

            var taxYear = new TaxYear();
            taxYear = _taxService.GetById(id);
            return taxYear;

        }


        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
        {

            return totalEarnings - totalDeduction;

        }

        
        public decimal OverTimeHours(decimal hoursWorked, decimal contractualHours)
        {

            if(hoursWorked < contractualHours)
            {

                _OverTimeHours = 0.00m;

            }
            else

            {

                _OverTimeHours = hoursWorked - contractualHours;

            }
            return _OverTimeHours;
            
        }

        public decimal OverTimerate(decimal hourlyRate)
        {
            //time
            _OverTimeRate = hourlyRate * 1.5m;

            return _OverTimeRate;

        }

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal UnionFees)
        {
            _TotalDeduction = tax + nic + studentLoanRepayment + UnionFees;
            return _TotalDeduction;
        }

        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        {
            _TotalEarnings = overtimeEarnings + contractualEarnings;
            return _TotalEarnings;
        }

        public decimal OverTimeEarnings(decimal overtimeRate, decimal overtimeHours)
        {
            return overtimeRate * overtimeHours;
        }
        //dung khong dong bo
        public async Task DeleteAsync(int id)
        {
            var pay = GetById(id);
            _context.PaymentRecords.Remove(pay);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PaymentRecord pay)
        {           
            _context.PaymentRecords.Update(pay);
            await _context.SaveChangesAsync();
        }
    }
}