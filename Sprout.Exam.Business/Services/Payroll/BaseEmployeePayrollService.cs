using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sprout.Exam.Data;
using System.Threading.Tasks;
using Sprout.Exam.Business.DataTransferObjects;
using AutoMapper;

namespace Sprout.Exam.Business
{
    public abstract class BaseEmployeePayrollService
    {
        private readonly IEmployeePayrollRepository _employeePayrollRepository;
        private readonly IMapper _mapper;

        public BaseEmployeePayrollService(IMapper mapper, IEmployeePayrollRepository repository)
        {
            _employeePayrollRepository = repository;
            _mapper = mapper;
        }

        public async virtual Task<EmployeePayrollDto> GetPayrollDetails(int employeeId)
        {
            var empPayroll = await _employeePayrollRepository.GetDetails(employeeId);
            return _mapper.Map<EmployeePayroll, EmployeePayrollDto>(empPayroll);
        }

    }
}
