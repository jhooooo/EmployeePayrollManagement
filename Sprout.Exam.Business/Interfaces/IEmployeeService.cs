
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Business
{
    public interface IEmployeeService
    {
        public Task<List<EmployeeDto>> Get();

        public Task<EmployeeDto> GetById(int id);

        public Task<int> Delete(int id);

        public Task<EmployeeDto> Create(CreateEmployeeDto employee);

        public Task<EmployeeDto> Update(EditEmployeeDto employee);

    }
}
