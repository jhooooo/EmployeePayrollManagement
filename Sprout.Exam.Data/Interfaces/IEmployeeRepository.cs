using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.Data.Models;

namespace Sprout.Exam.Data
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> Get();

        public Task<Employee> GetById(int id);

        public Task<int> Delete(int id);

        public Task<Employee> Create(Employee employee);

        public Task<Employee> Update(Employee employee);

    }
}
