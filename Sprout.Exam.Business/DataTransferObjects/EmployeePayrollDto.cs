using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class EmployeePayrollDto
    {
        public int EmployeePayrollId { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Tin { get; set; }
        public string Birthdate { get; set; }
        public int TypeId { get; set; }
        public decimal Rate { get; set; }
        public decimal NetIncome { get; set; }
        public decimal WorkedDays { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal Tax { get; set; }
    }
}
