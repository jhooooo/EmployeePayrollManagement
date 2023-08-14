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
    public class EmployeeValidatorTest
    {
        public EmployeeValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new EmployeeValidator();

        }

        [Test]
        public void Validation_Valid()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "Jane Doe",
                Tin = "123215413",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void FullName_ShouldNotBeEmpty()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = null,
                Tin = "123215413",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("FullName");
        }

        [Test]
        public void Tin_ShouldNotBeEmpty()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "John Doe",
                Tin = null,
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("Tin");
        }

        [Test]
        public void Tin_ShouldNotAllowCharacters()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "John Doe",
                Tin = "aaa",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("Tin");
        }

        [Test]
        public void Tin_ShouldBe9Digits()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = "1993-03-25",
                FullName = "John Doe",
                Tin = "1234",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("Tin");
        }

        [Test]
        public void BirthDate_ShouldNotBeEmpty()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = null,
                FullName = "John Doe",
                Tin = "123456789",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("Birthdate");
        }

        [Test]
        public void BirthDate_ShouldNotBeAFutureDate()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = DateTime.MaxValue.ToString(),
                FullName = "John Doe",
                Tin = "123456789",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("Birthdate");
        }

        [Test]
        public void BirthDate_ShouldBeAValidDate()
        {
            var employee = new CreateEmployeeDto
            {
                Birthdate = "test",
                FullName = "John Doe",
                Tin = "123456789",
                TypeId = 1
            };

            var result = validator.TestValidate(employee);

            result.ShouldHaveValidationErrorFor("Birthdate");
        }
    }
}
