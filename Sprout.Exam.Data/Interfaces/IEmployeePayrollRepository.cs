using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Data
{
    public interface IEmployeePayrollRepository
    {
        public Task<EmployeePayroll> GetDetails(int employeeId);
    }
}
