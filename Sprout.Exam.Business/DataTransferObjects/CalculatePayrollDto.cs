using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class CalculatePayrollDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public double Absences { get; set; }
        public double DaysWorked { get; set; }
    }
}
