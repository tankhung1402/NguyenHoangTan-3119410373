using Microsoft.EntityFrameworkCore;
using Pay1193.Entity;
using Pay1193.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class EmployeeService : IEmployee

    {
        private readonly ApplicationDbContext _context;
        private decimal studentLoanAmount;
        private decimal unionFee;
        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public Employee GetById(int id)
        {
            return _context.Employees.Where(employee => employee.Id == id).FirstOrDefault();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = GetById(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }


        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);
            if(employee.StudentLoan == StudentLoan.Yes)
            {
                if(totalAmount < 1750)
                {
                    studentLoanAmount = 0m;
                }
                else
                {
                    studentLoanAmount = totalAmount * 0.06m;
                }
            }
            else
            {
                studentLoanAmount = 0m;
            }
            return studentLoanAmount;
        }
        

        public decimal UnionFee(int id)
        {
            var employee = GetById(id);
            if(employee.UnionMember == UnionMember.Yes)
            {
                unionFee = 8m;
            }
            else
            {
                unionFee = 0m;
            }
            return unionFee;
        }

       

        
    }
}
