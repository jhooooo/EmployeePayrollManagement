using System;
using System.Collections.Generic;

#nullable disable

namespace Sprout.Exam.Data.Models
{
    public partial class EmployeePayroll
    {
        public int EmployeeId { get; set; }
        public decimal? Tax { get; set; }
        public decimal SalaryRate { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal WorkedDays { get; set; }
        public decimal? NetIncomeSalary { get; set; }
        public int EmployeePayrollId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
