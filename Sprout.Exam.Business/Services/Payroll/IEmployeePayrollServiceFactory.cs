using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business
{
    public interface IEmployeePayrollServiceFactory
    {
        public IEmployeePayrollService GetEmployeePayrollService(int employeeId);
    }
}
