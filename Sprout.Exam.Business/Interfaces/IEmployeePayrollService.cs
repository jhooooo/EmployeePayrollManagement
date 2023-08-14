using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Business
{
    public interface IEmployeePayrollService
    {
        public Task<EmployeePayrollDto> GetPayrollDetails(int employeeId);
        public Task<double> CalculateSalary(CalculatePayrollDto request);
    }
}
