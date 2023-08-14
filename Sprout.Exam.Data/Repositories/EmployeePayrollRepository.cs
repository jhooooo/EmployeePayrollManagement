using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Data.Models;
using System.Linq;

namespace Sprout.Exam.Data
{
    public class EmployeePayrollRepository : IEmployeePayrollRepository
    {
        public async Task<EmployeePayroll> GetDetails(int employeeId)
        {
            using (var context = new SproutExamDbContext())
            {
                return await context.EmployeePayrolls.Where(x => x.EmployeeId == employeeId).FirstOrDefaultAsync();
            }
        }
    }
}
