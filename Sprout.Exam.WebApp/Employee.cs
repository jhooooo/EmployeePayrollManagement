using System;
using System.Collections.Generic;

#nullable disable

namespace Sprout.Exam.WebApp
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeePayrolls = new HashSet<EmployeePayroll>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Tin { get; set; }
        public int EmployeeTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<EmployeePayroll> EmployeePayrolls { get; set; }
    }
}
