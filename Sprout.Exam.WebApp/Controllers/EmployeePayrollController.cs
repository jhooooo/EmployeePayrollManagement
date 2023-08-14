using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprout.Exam.Business;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePayrollController : ControllerBase
    {
        private IEmployeePayrollService _employeePayrollService;
        private readonly IEmployeePayrollServiceFactory _employeePayrollServiceFactory;
        private readonly IValidator<CalculatePayrollDto> _calculatePayrollValidator;

        public EmployeePayrollController(IEmployeePayrollServiceFactory employeePayrollServiceFactory, IValidator<CalculatePayrollDto> calculatePayrollDto)
        {
            _employeePayrollServiceFactory = employeePayrollServiceFactory;
            _calculatePayrollValidator = calculatePayrollDto;
        }

        [HttpGet("{empId}")]
        public async Task<IActionResult> GetDetails(int empId)
        {
            _employeePayrollService = _employeePayrollServiceFactory.GetEmployeePayrollService(empId);
            var result = await Task.FromResult(_employeePayrollService.GetPayrollDetails(empId));
            return Ok(result);
        }

        [HttpPost("{empId}/calculate")]
        public async Task<IActionResult> CalculatePay(CalculatePayrollDto request)
        {
            var validationResult = _calculatePayrollValidator.Validate(request);

            if (!validationResult.IsValid) { return BadRequest(validationResult); }

            _employeePayrollService = _employeePayrollServiceFactory.GetEmployeePayrollService(request.EmployeeId);
            var result = await Task.FromResult(_employeePayrollService.CalculateSalary(request));

            return Ok(result);
        }
    }
}
