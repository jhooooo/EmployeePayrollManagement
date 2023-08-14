using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.TestHelper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business;
using Sprout.Exam.Business.Validators;
using Moq;

namespace Sprout.Exam.Tests
{
    public class CalculatePayrollValidatorTest
    {
        public CalculatePayrollValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new CalculatePayrollValidator();

        }

        [Test]
        public void Validation_Valid()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = 0,
                DaysWorked = 0
            };

            var result = validator.TestValidate(employee);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Absences_ShouldBeDivisibleByPoint5()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = 1.2,
                DaysWorked = 0
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor(x => x.Absences);
        }

        [Test]
        public void Absences_ShouldNotExceed22()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = 23,
                DaysWorked = 0
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor(x => x.Absences);
        }

        [Test]
        public void Absences_ShouldNotBeNegative()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = -1,
                DaysWorked = 0
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor(x => x.Absences);
        }

        [Test]
        public void WorkedDays_ShouldBeDivisibleByPoint5()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = 0,
                DaysWorked = 1.2
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor(x => x.DaysWorked);
        }

        [Test]
        public void WorkedDays_ShouldNotExceed22()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = 0,
                DaysWorked = 23
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor(x => x.DaysWorked);
        }

        [Test]
        public void WorkedDays_ShouldNotBeNegative()
        {
            var employee = new CalculatePayrollDto
            {
                Absences = 0,
                DaysWorked = -1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor(x => x.DaysWorked);
        }
    }
}
