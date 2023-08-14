using AutoMapper;
using Moq;
using NUnit.Framework;
using Sprout.Exam.Business;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Tests
{
    public class ContractualPayrollServiceTest
    {
        private ContractualPayrollService ContractualPayrollService;
        private Mock<IEmployeeService> _employeeServiceMock;
        private Mock<IEmployeePayrollRepository> _employeePayrollRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private EmployeeDto employeedto;
        private CalculatePayrollDto _calculatePayrollDto;
             

        [SetUp]
        public void Setup()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _employeePayrollRepositoryMock = new Mock<IEmployeePayrollRepository>();
            _mapperMock = new Mock<IMapper>();

            employeedto = new EmployeeDto
            {
                Id = 1,
                FullName = "Juan Dela Cruz",
                Birthdate = "1994-05-05",
                Tin = "123456789",
                TypeId = 1
            };

            _calculatePayrollDto = new CalculatePayrollDto();

            ContractualPayrollService = new ContractualPayrollService(_employeeServiceMock.Object, _employeePayrollRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetPayrollDetails_Success()
        {
            var empId = 1;
            _employeeServiceMock.Setup(x => x.GetById(empId)).ReturnsAsync(employeedto);

            var response = await ContractualPayrollService.GetPayrollDetails(empId);

            Assert.True(response is EmployeePayrollDto);

            var employeePayroll = response as EmployeePayrollDto;

            Assert.AreEqual(employeePayroll.Rate, 500);
        }

        [Test]
        public async Task CalculateSalary_22WorkedDays_ShouldReturn11000()
        {
            _calculatePayrollDto.DaysWorked = 22;

            _employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(employeedto);

            var response = await ContractualPayrollService.CalculateSalary(_calculatePayrollDto);

            Assert.AreEqual(response, 11000);
        }

        [Test]
        public async Task CalculateSalary_18andHaldWorkedDays_ShouldReturn9250()
        {
            _calculatePayrollDto.DaysWorked = 18.5;

            _employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(employeedto);

            var response = await ContractualPayrollService.CalculateSalary(_calculatePayrollDto);

            Assert.AreEqual(response, 9250);
        }
    }
}
