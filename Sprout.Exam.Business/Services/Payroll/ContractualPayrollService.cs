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
    public class ContractualPayrollService : BaseEmployeePayrollService, IEmployeePayrollService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeePayrollRepository _employeePayrollRepository;
        private readonly IMapper _mapper;

        public ContractualPayrollService(IEmployeeService employeeService,
                                        IEmployeePayrollRepository employeePayrollRepository,
                                        IMapper mapper) : base(mapper, employeePayrollRepository)
        {
            _employeeService = employeeService;
            _employeePayrollRepository = employeePayrollRepository;
            _mapper = mapper;

        }

        public async Task<double> CalculateSalary(CalculatePayrollDto req)
        {
            var payrollDetails = await GetPayrollDetails(req.EmployeeId);

            var salary = (Double)payrollDetails.Rate * ((req.DaysWorked > 0) ? req.DaysWorked : 0);

            return Math.Round(salary, 2);
            
        }

        public async override Task<EmployeePayrollDto> GetPayrollDetails(int employeeId)
        {
            var payrollDetails = new EmployeePayrollDto()
            {
                EmployeePayrollId = 1,
                EmployeeId = employeeId,
                Rate = 500
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