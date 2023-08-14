using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Sprout.Exam.Common;
using Sprout.Exam.Business.DataTransferObjects;
using AutoMapper;
using FluentValidation;
using Sprout.Exam.Common.Exceptions;
using Sprout.Exam.Data;
using System.Threading.Tasks;
using System.Data.Common;
using System.Linq;

namespace Sprout.Exam.Business
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IMapper mapper, IEmployeeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<EmployeeDto> Create(CreateEmployeeDto employee)
        {
            try
            {
                if (employee == null) throw new ArgumentNullException();

                var emp = _mapper.Map<CreateEmployeeDto, Employee>(employee);
                if (_repository.Get().Result.Any(x => x.FullName.Trim() == employee.FullName.Trim()))
                {
                    throw new DuplicateException("Employee already exists!");
                }
                var createdEmp = await _repository.Create(emp);
                return _mapper.Map<Employee, EmployeeDto>(createdEmp);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeDto> Update(EditEmployeeDto employee)
        {
            try
            {
                if (employee == null) throw new ArgumentNullException();

                var emp = _mapper.Map<EditEmployeeDto, Employee>(employee);

                if (await _repository.GetById(employee.Id) == null)
                {
                    throw new KeyNotFoundException("Employee does not exists!");
                }


                var updatedEmp = await _repository.Update(emp);
                return _mapper.Map<Employee, EmployeeDto>(updatedEmp);

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
                return await _repository.Delete(id);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EmployeeDto>> Get()
        {
            try
            {
                var empList = await _repository.Get();
                empList = empList.Where(x => !x.IsDeleted).ToList();
                var empListDto = _mapper.Map<List<Employee>, List<EmployeeDto>>(empList);
                return await Task.FromResult(empListDto);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeDto> GetById(int id)
        {
            try
            {
                var emp = await _repository.GetById(id);

                if (emp == null)
                {
                    throw new KeyNotFoundException("Employee does not exists!");
                }

                var empDto = _mapper.Map<Employee, EmployeeDto>(emp);
                return await Task.FromResult(empDto);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
