using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Persistence.Migrations;
using Pay1193.Service;
using Pay1193.Service.Implementation;
using SelectPdf;

using static System.Net.WebRequestMethods;
using System.IO;
using System.Security.Policy;
using System.Drawing.Printing;
using System;


namespace Pay1193.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IPayService _payService;
        private readonly IEmployee _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private decimal _OvertimeHours;
        private decimal _ContractualEarnings;
        private decimal _OvertimeEarnings;
        private decimal _TotalEarnings;
        private decimal _Tax;
        private decimal _UnionFee;
        private decimal _StudentLoan;
        private decimal _NationalInsurance;
        private decimal _TotalDeduction;
        public PaymentController(IPayService payService,IEmployee employeeService, ITaxService taxService, INationalInsuranceService nationalInsuranceService)
        {
            _payService = payService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceService = nationalInsuranceService;
        }
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.taxYears = _payService.GetAllTaxYear();
            var employee = _employeeService.GetById(id);
            var model = new PaymentCreateViewModel()
            {
                EmployeeId=employee.Id,
                NationalInsuranceNo=employee.NationalInsuranceNo,
            };         
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PaymentCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                var payrecord = new PaymentRecord
                {
                    //Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    NiNo = model.NationalInsuranceNo,
                    DatePay = model.DatePay.Date,
                    MonthPay = model.MonthPay,
                    TaxYearId = model.TaxYearId,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HourlyWorked = model.HourlyWorked,
                    ContractualHours = model.ContractualHours,
                    OverTimeHours = _OvertimeHours = _payService.OverTimeHours(model.HourlyWorked, model.ContractualHours),
                    ContractualEarnings = _ContractualEarnings = _payService.ContractualEarnings(model.ContractualHours, model.HourlyWorked, model.HourlyRate),
                    OvertimeEarnings = _OvertimeEarnings = _payService.OverTimeEarnings(_payService.OverTimerate(model.HourlyRate), _OvertimeHours),
                    TotalEarnings = _TotalEarnings = _payService.TotalEarnings(_OvertimeEarnings, _ContractualEarnings),
                    Tax = _Tax = _taxService.TaxAmount(_TotalEarnings),
                    NiC = _NationalInsurance = _nationalInsuranceService.NIContribution(_TotalEarnings),
                    UnionFee = _UnionFee = _employeeService.UnionFee(model.EmployeeId),
                    SLC = _StudentLoan = _employeeService.StudentLoanRepaymentAmount(model.EmployeeId,_TotalEarnings),
                    TotalDeduction = _TotalDeduction = _payService.TotalDeduction(_Tax,_NationalInsurance,_StudentLoan,_UnionFee),
                    NetPayment = _payService.NetPay(_TotalEarnings, _TotalDeduction)
                };
                await _payService.CreateAsync(payrecord);
                return RedirectToAction("Index");
            }
            ViewBag.taxYears = _payService.GetAllTaxYear();
            return View();
        }
        
        public IActionResult Index()
        {

            var model = _payService.GetAll().Select(pay => new PaymentIndexViewModel
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                FullName = _employeeService.GetById(pay.EmployeeId).FullName,
                DatePay = pay.DatePay,
                MonthPay = pay.MonthPay,
                TaxYearId = pay.TaxYearId,
                Year =  _taxService.GetById(pay.TaxYearId).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.TotalDeduction,
                NetPay = pay.NetPayment
            }).ToList();
            return View(model);      
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var pay = _payService.GetById(id);
            if (pay == null)
            {
                return NotFound();
            }
            var model = new PaymentDeleteViewModel
            {
                Id = pay.Id,
                FullName = _employeeService.GetById(pay.EmployeeId).FullName
                
            };
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(PaymentDeleteViewModel model)
        {
            await _payService.DeleteAsync(model.Id);
            return RedirectToAction("Index");
        }


/*
        HtmlToPdf converter = new HtmlToPdf();

        var url = "https://localhost:44370/Payment/ViewPDF/" + id.ToString();
        // set converter options


        // create a new pdf document converting an url
        PdfDocument doc = converter.ConvertUrl(url);


        // save pdf document
        doc.Save("PaymentBill.pdf");

            // close pdf document
            doc.Close();*/
        public IActionResult ViewPDF(int id)
        {
            var pay = _payService.GetById(id);
            var model = new PaymentDetailViewModel()
            {
                Id = pay.Id,
                FullName = _employeeService.GetById(pay.EmployeeId).FullName,
                EmployeeId = pay.EmployeeId,
                NiNo = pay.NiNo,
                PayDate = pay.DatePay,
                PayMonth = pay.MonthPay,
                Year = _taxService.GetById(pay.TaxYearId).YearOfTax,
                TaxCode = pay.TaxCode,
                HourlyRate = pay.HourlyRate,
                HourlyWorked = pay.HourlyWorked,
                ContractualHours = pay.ContractualHours,
                OvertimeHours = pay.OverTimeHours,
                OvertimeRate = _payService.OverTimeHours(pay.HourlyWorked, pay.ContractualHours),
                ContractualEarnings = pay.ContractualEarnings,
                OvertimeEarnings = pay.OvertimeEarnings,
                Tax = pay.Tax,
                NiC = pay.NiC,
                UnionFee = pay.UnionFee,
                SLC = pay.SLC,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.TotalDeduction,
                NetPayment = pay.NetPayment
            };                               
            return View(model);           
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var pay = _payService.GetById(id);
            var model = new PaymentDetailViewModel()
            {
                Id = pay.Id,
                FullName = _employeeService.GetById(pay.EmployeeId).FullName,
                EmployeeId = pay.EmployeeId,
                NiNo = pay.NiNo,
                PayDate = pay.DatePay,
                PayMonth = pay.MonthPay,
                Year = _taxService.GetById(pay.TaxYearId).YearOfTax,
                TaxCode = pay.TaxCode,
                HourlyRate = pay.HourlyRate,
                HourlyWorked = pay.HourlyWorked,
                ContractualHours = pay.ContractualHours,
                OvertimeHours = pay.OverTimeHours,
                OvertimeRate = _payService.OverTimeHours(pay.HourlyWorked, pay.ContractualHours),
                ContractualEarnings = pay.ContractualEarnings,
                OvertimeEarnings = pay.OvertimeEarnings,
                Tax = pay.Tax,
                NiC = pay.NiC,
                UnionFee = pay.UnionFee,
                SLC = pay.SLC,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.TotalDeduction,
                NetPayment = pay.NetPayment
            };
            return View(model);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.taxYears = _payService.GetAllTaxYear();
            var pay = _payService.GetById(id);
            if(pay == null)
            {
                return NotFound();
            }
            var model = new PaymentEditViewModel()
            {
                Id = pay.Id,             
                NationalInsuranceNo = pay.NiNo,
                DatePay = pay.DatePay,
                MonthPay = pay.MonthPay,
                TaxYearId = pay.TaxYearId,              
                TaxCode = pay.TaxCode,
                HourlyRate = pay.HourlyRate,
                HourlyWorked = pay.HourlyWorked,
                ContractualHours = pay.ContractualHours
            }; 
            
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PaymentEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var pay = _payService.GetById(model.Id);
                if(pay == null)
                {
                    return NotFound();
                }
            
                pay.NiNo = model.NationalInsuranceNo;
                pay.DatePay = model.DatePay;
                pay.MonthPay = model.MonthPay;
                pay.TaxYearId = model.TaxYearId;
                pay.TaxCode = model.TaxCode;
                pay.HourlyRate = model.HourlyRate;
                pay.HourlyWorked = model.HourlyWorked;
                pay.ContractualHours = model.ContractualHours;
                pay.OverTimeHours = _OvertimeHours = _payService.OverTimeHours(model.HourlyWorked, model.ContractualHours);
                pay.ContractualEarnings = _ContractualEarnings = _payService.ContractualEarnings(model.ContractualHours, model.HourlyWorked, model.HourlyRate);
                pay.OvertimeEarnings = _OvertimeEarnings = _payService.OverTimeEarnings(_payService.OverTimerate(model.HourlyRate), _OvertimeHours);
                pay.TotalEarnings = _TotalEarnings = _payService.TotalEarnings(_OvertimeEarnings, _ContractualEarnings);
                pay.Tax = _Tax = _taxService.TaxAmount(_TotalEarnings);
                pay.NiC = _NationalInsurance = _nationalInsuranceService.NIContribution(_TotalEarnings);
                pay.UnionFee = _UnionFee = _employeeService.UnionFee(pay.EmployeeId);
                pay.SLC = _StudentLoan = _employeeService.StudentLoanRepaymentAmount(pay.EmployeeId, _TotalEarnings);
                pay.TotalDeduction = _TotalDeduction = _payService.TotalDeduction(_Tax, _NationalInsurance, _StudentLoan, _UnionFee);
                pay.NetPayment = _payService.NetPay(_TotalEarnings, _TotalDeduction);

                await _payService.UpdateAsync(pay);
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
