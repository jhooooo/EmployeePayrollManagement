using AutoMapper;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Business
{
    public class EmployeePayrollServiceFactory : IEmployeePayrollServiceFactory
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeePayrollRepository _employeePayrollRepository;
        private readonly IMapper _mapper;
        public EmployeePayrollServiceFactory(IEmployeeService employeeService
                                                , IEmployeePayrollRepository employeePayrollRepository
                                                , IMapper mapper)
        {
            this._employeeService = employeeService;
            this._employeePayrollRepository = employeePayrollRepository;
            this._mapper = mapper;
        }

        public IEmployeePayrollService GetEmployeePayrollService(int employeeId)
        {
            IEmployeePayrollService payrollService = null;
            var employee = _employeeService.GetById(employeeId);

            switch (employee.Result.TypeId)
            {
                case (int)EmployeeType.Contractual:
                    return new ContractualPayrollService(this._employeeService, this._employeePayrollRepository, _mapper);
                case (int)EmployeeType.Regular:
                    return new RegularPayrollService(this._employeeService, this._employeePayrollRepository, _mapper);
                default:
                    throw new ApplicationException(string.Format("Employee Payroll Service '{0}' cannot be created", payrollService));
            }
        }
    }
}
