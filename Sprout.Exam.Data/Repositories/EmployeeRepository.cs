using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Data.Models;

namespace Sprout.Exam.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SproutExamDbContext _dbContext;
        public EmployeeRepository(SproutExamDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Employee>> Get()
        {
            try
            {
                var list = await _dbContext.Employees.AsNoTracking().ToListAsync();
                return list;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employee> GetById(int id)
        {
            try
            {
                return await _dbContext.Employees.FindAsync(id);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employee> Create(Employee employee)
        {
            try
            {
                var dbEntity = _dbContext.Entry<Employee>(employee);
                await _dbContext.Set<Employee>().AddAsync(employee);
                await _dbContext.SaveChangesAsync();
                return employee;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employee> Update(Employee employee)
        {
            try
            {
                var dbEntity = _dbContext.Employees.Add(employee);
                dbEntity.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return employee;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> Delete(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null) throw new KeyNotFoundException();
                employee.IsDeleted = true;
                var dbEntity = _dbContext.Entry<Employee>(employee);
                dbEntity.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return id;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
