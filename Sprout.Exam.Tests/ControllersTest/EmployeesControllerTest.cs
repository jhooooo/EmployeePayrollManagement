using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Sprout.Exam.Business;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using FluentValidation.Results;

namespace Sprout.Exam.Tests
{
    public class EmployeesControllerTest
    {
        private EmployeesController employeeController;
        private Mock<IEmployeeService> _employeeServiceMock;
        private Mock<IValidator<EditEmployeeDto>> _editvalidatorMock;
        private Mock<IValidator<CreateEmployeeDto>> _createValidatorMock;
        private EditEmployeeDto editEmployeeMock;
        private CreateEmployeeDto createEmployeeMock;
        private EmployeeDto employeeMock;
        private List<EmployeeDto> employeeList;
        private ValidationResult validationResultValid;
        private ValidationResult validationResultWithError;

        [SetUp]
        public void Setup()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _editvalidatorMock = new Mock<IValidator<EditEmployeeDto>>();
            _createValidatorMock = new Mock<IValidator<CreateEmployeeDto>>();

            employeeList = new List<EmployeeDto>();

            employeeList.Add(new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            });

            employeeList.Add(new EmployeeDto
            {
                Birthdate = "1993-05-28",
                FullName = "John Doe",
                Id = 2,
                Tin = "957125412",
                TypeId = 2
            });

            validationResultValid = new ValidationResult();
            validationResultWithError = new ValidationResult(new List<ValidationFailure>()
                {
                    new ValidationFailure("Full Name","Required"){ErrorCode = "1001"}
                });

            employeeController = new EmployeesController(_employeeServiceMock.Object, _editvalidatorMock.Object, _createValidatorMock.Object);
        }

        [Test]
        public async Task Get_ReturnsList()
        {
            _employeeServiceMock.Setup(x => x.Get()).Returns(Task.FromResult(employeeList));

            var response = await employeeController.Get();

            Assert.True(response is OkObjectResult);

            var responseResult = response as OkObjectResult;

            Assert.True(responseResult.Value is List<EmployeeDto>);

            var responseValue = responseResult.Value as List<EmployeeDto>;

            Assert.NotNull(responseValue);
            Assert.IsNotEmpty(responseValue);
            Assert.AreEqual(employeeList, responseValue);
        }

        [Test]
        public async Task Get_ReturnsNullList()
        {
            _employeeServiceMock.Setup(x => x.Get()).Returns(Task.FromResult<List<EmployeeDto>>(null));

            var response = await employeeController.Get();

            Assert.True(response is OkObjectResult);

            var responseResult = response as OkObjectResult;

            Assert.IsNull(responseResult.Value);
        }

        [Test]
        public async Task Get_ServiceThrowsException()
        {
            _employeeServiceMock.Setup(x => x.Get()).ThrowsAsync(new Exception());

            var response = await employeeController.Get();

            Assert.True(response is BadRequestResult);
        }

        [Test]
        public async Task GetById_Ok()
        {
            foreach (var employee in employeeList)
            {
                _employeeServiceMock.Setup(x => x.GetById(employee.Id)).ReturnsAsync(employee);

                var response = await employeeController.GetById(employee.Id);

                Assert.True(response is OkObjectResult);

                var responseResult = response as OkObjectResult;

                Assert.True(responseResult.Value is EmployeeDto);

                var responseValue = responseResult.Value as EmployeeDto;

                Assert.NotNull(responseValue);
                Assert.AreEqual(employee, responseValue);
            }
        }

        [Test]
        public async Task GetById_NonExistent()
        {
            _employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult<EmployeeDto>(null));

            var response = await employeeController.GetById(5);

            Assert.True(response is NotFoundResult);
        }

        [Test]
        public async Task GetById_ServiceThrowsException()
        {
            _employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await employeeController.GetById(5);

            Assert.True(response is BadRequestResult);
        }

        [Test]
        public async Task Put_Ok()
        {
            editEmployeeMock = new EditEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            employeeMock = new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            _editvalidatorMock.Setup(x => x.Validate(It.IsAny<EditEmployeeDto>())).Returns(validationResultValid);
            _employeeServiceMock.Setup(x => x.Update(editEmployeeMock)).Returns(Task.FromResult(employeeMock));

            var response = await employeeController.Put(editEmployeeMock);

            Assert.True(response is OkObjectResult);

            var responseResult = response as OkObjectResult;

            Assert.True(responseResult.Value is EmployeeDto);

            var responseValue = responseResult.Value as EmployeeDto;

            Assert.NotNull(responseValue);
            Assert.AreEqual(employeeMock, responseValue);

        }

        [Test]
        public async Task Put_ValidationHasError()
        {
            editEmployeeMock = new EditEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            employeeMock = new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            _editvalidatorMock.Setup(x => x.Validate(It.IsAny<EditEmployeeDto>())).Returns(validationResultWithError);
            _employeeServiceMock.Setup(x => x.Update(editEmployeeMock)).Returns(Task.FromResult(employeeMock));

            var response = await employeeController.Put(editEmployeeMock);

            Assert.True(response is BadRequestObjectResult);

            var responseResult = response as BadRequestObjectResult;

            Assert.True(responseResult.Value is ValidationResult);

            var responseValue = responseResult.Value as ValidationResult;

            Assert.NotNull(responseValue);
            Assert.AreEqual(validationResultWithError, responseValue);

        }

        [Test]
        public async Task Put_ServiceThrowsException()
        {
            editEmployeeMock = new EditEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            employeeMock = new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            _editvalidatorMock.Setup(x => x.Validate(It.IsAny<EditEmployeeDto>())).Returns(validationResultValid);
            _employeeServiceMock.Setup(x => x.Update(editEmployeeMock)).ThrowsAsync(new Exception());

            var response = await employeeController.Put(editEmployeeMock);

            Assert.True(response is BadRequestResult);
        }

        [Test]
        public async Task Post_Ok()
        {
            createEmployeeMock = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Tin = "123215413",
                TypeId = 1
            };

            employeeMock = new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            _createValidatorMock.Setup(x => x.Validate(It.IsAny<CreateEmployeeDto>())).Returns(validationResultValid);
            _employeeServiceMock.Setup(x => x.Create(createEmployeeMock)).Returns(Task.FromResult(employeeMock));

            var response = await employeeController.Post(createEmployeeMock);

            Assert.True(response is CreatedResult);

            var responseResult = response as CreatedResult;

            Assert.NotNull(responseResult.Location);
            Assert.AreEqual("/api/employees/1", responseResult.Location);
            Assert.AreEqual(1, responseResult.Value);

        }

        [Test]
        public async Task Post_ValidationHasError()
        {
            createEmployeeMock = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Tin = "123215413",
                TypeId = 1
            };

            employeeMock = new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            _createValidatorMock.Setup(x => x.Validate(It.IsAny<CreateEmployeeDto>())).Returns(validationResultWithError);
            _employeeServiceMock.Setup(x => x.Create(createEmployeeMock)).Returns(Task.FromResult(employeeMock));

            var response = await employeeController.Post(createEmployeeMock);

            Assert.True(response is BadRequestObjectResult);

            var responseResult = response as BadRequestObjectResult;

            Assert.True(responseResult.Value is ValidationResult);

            var responseValue = responseResult.Value as ValidationResult;

            Assert.NotNull(responseValue);
            Assert.AreEqual(validationResultWithError, responseValue);

        }

        [Test]
        public async Task Post_ServiceThrowsException()
        {
            createEmployeeMock = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Tin = "123215413",
                TypeId = 1
            };

            employeeMock = new EmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Id = 1,
                Tin = "123215413",
                TypeId = 1
            };

            _createValidatorMock.Setup(x => x.Validate(It.IsAny<CreateEmployeeDto>())).Returns(validationResultValid);
            _employeeServiceMock.Setup(x => x.Create(createEmployeeMock)).ThrowsAsync(new Exception());

            var response = await employeeController.Post(createEmployeeMock);

            Assert.True(response is BadRequestResult);
        }

        [Test]
        public async Task Delete_Ok()
        {
            var empId = 1;

            _employeeServiceMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(empId);

            var response = await employeeController.Delete(empId);

            Assert.True(response is OkObjectResult);

            var responseResult = response as OkObjectResult;

            Assert.AreEqual(empId, responseResult.Value);
        }

        [Test]
        public async Task Delete_ServiceReturnsError()
        {
            var empId = 1;

            _employeeServiceMock.Setup(x => x.Delete(It.IsAny<int>())).ThrowsAsync(new Exception());

            var response = await employeeController.Delete(empId);

            Assert.True(response is BadRequestResult);
        }
    }
}