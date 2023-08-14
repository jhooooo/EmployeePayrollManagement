using AutoMapper;
using Moq;
using NUnit.Framework;
using Sprout.Exam.Business;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Exceptions;
using Sprout.Exam.Data;
using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests
{
    public class EmployeeServiceTest
    {
        private EmployeeService employeeService;
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IMapper> _mapperMock;

        private EditEmployeeDto editEmployeeMock;
        private CreateEmployeeDto createEmployeeMock;
        private EmployeeDto employeeDtoMock;
        private Employee employeeMock;
        private List<Employee> employeeList;

        [SetUp]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();

            employeeList = new List<Employee>();

            

            employeeList.Add(new Employee
            {
                Birthdate = new DateTime(1990, 12, 24),
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                EmployeeTypeId = 1
            });

            employeeList.Add(new Employee
            {
                Birthdate = new DateTime(1990, 10, 24),
                FullName = "John Doe",
                Id = 2,
                Tin = "957125412",
                EmployeeTypeId = 2,
                IsDeleted = true
            });

            employeeService = new EmployeeService(_mapperMock.Object, _employeeRepositoryMock.Object);
        }

        [Test]
        public async Task Create_Ok()
        {
            createEmployeeMock = new CreateEmployeeDto
            {
                FullName = "Juan Dela Cruz",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            _mapperMock.Setup(x => x.Map<CreateEmployeeDto, Employee>(It.IsAny<CreateEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.Get()).Returns(Task.FromResult(new List<Employee>()));
            _employeeRepositoryMock.Setup(x => x.Create(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee()));

            var response = await employeeService.Create(createEmployeeMock);

            Assert.True(response is EmployeeDto);
        }

        [Test]
        public async Task Create_NullEmployee()
        {
            _mapperMock.Setup(x => x.Map<CreateEmployeeDto, Employee>(It.IsAny<CreateEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.Get()).Returns(Task.FromResult(new List<Employee>()));
            _employeeRepositoryMock.Setup(x => x.Create(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee()));

            Assert.ThrowsAsync<ArgumentNullException>(async () => await employeeService.Create(null));

        }

        [Test]
        public async Task Create_AddAlreadyExistingFullName()
        {
            createEmployeeMock = new CreateEmployeeDto
            {
                FullName = "John Doe ",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            employeeMock = new Employee
            {
                FullName = "Juan Dela Cruz"
            };

            _mapperMock.Setup(x => x.Map<CreateEmployeeDto, Employee>(It.IsAny<CreateEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.Get()).Returns(Task.FromResult(employeeList));
            _employeeRepositoryMock.Setup(x => x.Create(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee()));

            Assert.ThrowsAsync<DuplicateException>(async () => await employeeService.Create(createEmployeeMock));
        }

        [Test]
        public async Task Create_RepoThrowsException()
        {
            createEmployeeMock = new CreateEmployeeDto
            {
                FullName = "Juan Dela Cruz",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            _mapperMock.Setup(x => x.Map<CreateEmployeeDto, Employee>(It.IsAny<CreateEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.Get()).Returns(Task.FromResult(new List<Employee>()));
            _employeeRepositoryMock.Setup(x => x.Create(It.IsAny<Employee>())).ThrowsAsync(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () => await employeeService.Create(createEmployeeMock));
        }

        [Test]
        public async Task Update_Ok() 
        {
            editEmployeeMock = new EditEmployeeDto
            {
                Id = 1,
                FullName = "Juan Dela Cruz",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            _mapperMock.Setup(x => x.Map<EditEmployeeDto, Employee>(It.IsAny<EditEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(editEmployeeMock.Id)).Returns(Task.FromResult(employeeList.Find(x => x.Id == editEmployeeMock.Id)));
            _employeeRepositoryMock.Setup(x => x.Update(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee()));

            var response = await employeeService.Update(editEmployeeMock);

            Assert.True(response is EmployeeDto);

        }

        [Test]
        public async Task Update_NullEmployee()
        {
            _mapperMock.Setup(x => x.Map<EditEmployeeDto, Employee>(It.IsAny<EditEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(editEmployeeMock.Id)).Returns(Task.FromResult(employeeList.Find(x => x.Id == editEmployeeMock.Id)));
            _employeeRepositoryMock.Setup(x => x.Update(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee()));

            Assert.ThrowsAsync<ArgumentNullException>(async () => await employeeService.Update(null));

        }

        [Test]
        public async Task Update_NonExistentEmployee()
        {
            editEmployeeMock = new EditEmployeeDto
            {
                Id = 6,
                FullName = "Juan Dela Cruz",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            _mapperMock.Setup(x => x.Map<EditEmployeeDto, Employee>(It.IsAny<EditEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(editEmployeeMock.Id)).Returns(Task.FromResult(employeeList.Find(x => x.Id == editEmployeeMock.Id)));
            _employeeRepositoryMock.Setup(x => x.Update(It.IsAny<Employee>())).Returns(Task.FromResult(new Employee()));

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await employeeService.Update(editEmployeeMock));
        }

        [Test]
        public async Task Update_RepoThrowsException()
        {
            editEmployeeMock = new EditEmployeeDto
            {
                Id = 1,
                FullName = "Juan Dela Cruz",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            _mapperMock.Setup(x => x.Map<EditEmployeeDto, Employee>(It.IsAny<EditEmployeeDto>())).Returns(new Employee());
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(editEmployeeMock.Id)).Returns(Task.FromResult(employeeList.Find(x => x.Id == editEmployeeMock.Id)));
            _employeeRepositoryMock.Setup(x => x.Update(It.IsAny<Employee>())).ThrowsAsync(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () => await employeeService.Update(editEmployeeMock));

        }

        [Test]
        public async Task GetById_Ok()
        {
            var empId = 1;
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(empId)).Returns(Task.FromResult(employeeList.Find(x => x.Id == empId)));

            var response = await employeeService.GetById(empId);

            Assert.True(response is EmployeeDto);
        }

        [Test]
        public async Task GetById_NonExistentEmployee()
        {
            var empId = 6;
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(empId)).Returns(Task.FromResult(employeeList.Find(x => x.Id == empId)));

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await employeeService.GetById(empId));
        }

        [Test]
        public async Task GetById_RepoThrowsException()
        {
            var empId = 1;
            _mapperMock.Setup(x => x.Map<Employee, EmployeeDto>(It.IsAny<Employee>())).Returns(new EmployeeDto());
            _employeeRepositoryMock.Setup(x => x.GetById(empId)).ThrowsAsync(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () => await employeeService.GetById(empId));
        }

        [Test]
        public async Task Get_Ok()
        {
            _mapperMock.Setup(x => x.Map<List<Employee>, List<EmployeeDto>>(It.IsAny<List<Employee>>())).Returns(new List<EmployeeDto>());
            _employeeRepositoryMock.Setup(x => x.Get()).Returns(Task.FromResult(employeeList));

            var response = await employeeService.Get();

            Assert.True(response is List<EmployeeDto>);
        }

        [Test]
        public async Task Get_RepoThrowsException()
        {
            _mapperMock.Setup(x => x.Map<List<Employee>, List<EmployeeDto>>(It.IsAny<List<Employee>>())).Returns(new List<EmployeeDto>());
            _employeeRepositoryMock.Setup(x => x.Get()).ThrowsAsync(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () => await employeeService.Get());
        }

        [Test]
        public async Task Delete_Ok()
        {
            _mapperMock.Setup(x => x.Map<List<Employee>, List<EmployeeDto>>(It.IsAny<List<Employee>>())).Returns(new List<EmployeeDto>());
            _employeeRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(Task.FromResult(1));

            var result = await employeeService.Delete(1);

            Assert.IsTrue(result > 0);
        }

        [Test]
        public async Task Delete_RepoThrowsException()
        {
            _mapperMock.Setup(x => x.Map<List<Employee>, List<EmployeeDto>>(It.IsAny<List<Employee>>())).Returns(new List<EmployeeDto>());
            _employeeRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () => await employeeService.Delete(1));
        }
    }
}
