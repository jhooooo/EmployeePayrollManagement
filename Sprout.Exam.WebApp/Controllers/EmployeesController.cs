using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Business;
using FluentValidation;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<EditEmployeeDto> _editvalidator;
        private readonly IValidator<CreateEmployeeDto> _createValidator;

        public EmployeesController(IEmployeeService employeeService,
                                    IValidator<EditEmployeeDto> editValidator,
                                    IValidator<CreateEmployeeDto> createValidator)
        {
            _employeeService = employeeService;
            _editvalidator = editValidator;
            _createValidator = createValidator;
        }

        /// <summary>2
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Task.FromResult(_employeeService.Get());
            if (result.Status == TaskStatus.Faulted) { return BadRequest(); }

            return Ok(result.Result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Task.FromResult(_employeeService.GetById(id));

            if (result.Status == TaskStatus.Faulted) { return BadRequest(); }

            if (result.Result == null) { return NotFound(); }

            return Ok(result.Result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            var validationResult = _editvalidator.Validate(input);

            if (!validationResult.IsValid) { return BadRequest(validationResult); }

            var result = await Task.FromResult(_employeeService.Update(input));

            if (result.Status == TaskStatus.Faulted) { return BadRequest(); }

            return Ok(result.Result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            var validationResult = _createValidator.Validate(input);

            if (!validationResult.IsValid) { return BadRequest(validationResult); }

            var result = await Task.FromResult(_employeeService.Create(input));

            if (result.Status == TaskStatus.Faulted) { return BadRequest(); }

            return Created($"/api/employees/{result.Result.Id}", result.Result.Id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Task.FromResult(_employeeService.Delete(id));
            
            if (result.Status == TaskStatus.Faulted) { return BadRequest(); }

            return Ok(id);
        }
    }
}
