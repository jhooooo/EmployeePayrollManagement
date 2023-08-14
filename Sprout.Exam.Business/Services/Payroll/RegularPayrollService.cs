using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Data;
using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Business
{
    public class RegularPayrollService : BaseEmployeePayrollService, IEmployeePayrollService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeePayrollRepository _employeePayrollRepository;
        private readonly IMapper _mapper;
        private readonly int monthlyWorkingDays;

        public RegularPayrollService(IEmployeeService employeeService,
                                        IEmployeePayrollRepository employeePayrollRepository,
                                        IMapper mapper) : base(mapper, employeePayrollRepository)
        {
            _employeeService = employeeService;
            _employeePayrollRepository = employeePayrollRepository;
            _mapper = mapper;

            //can be pulled out from reference data
            monthlyWorkingDays = 22;
        }

        public async Task<double> CalculateSalary(CalculatePayrollDto req)
        {

            var payrollDetails = await GetPayrollDetails(req.EmployeeId);

            var absencesSalaryDeduction = (req.Absences > 0) ? ((Double)payrollDetails.Rate / monthlyWorkingDays) * req.Absences : 0;

            var salary = ((Double)payrollDetails.Rate - absencesSalaryDeduction) * (1 - (Double)(payrollDetails.Tax)/100);

            return Math.Round(salary, 2);

        }

        public async override Task<EmployeePayrollDto> GetPayrollDetails(int employeeId)
        {
            var payrollDetails = new EmployeePayrollDto()
            {
                EmployeePayrollId = 1,
                EmployeeId = employeeId,
                Rate = 20000,
                Tax = 12
            };

            var emp = await _employeeService.GetById(employeeId);

            payrollDetails.FullName = emp.FullName;
            payrollDetails.Tin = emp.Tin;
            payrollDetails.Birthdate = emp.Birthdate;
            payrollDetails.TypeId = emp.TypeId;

            return payrollDetails;
        }
    }
}
