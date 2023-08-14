using FluentValidation;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business
{
    public class EmployeeValidator: AbstractValidator<BaseSaveEmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();

            RuleFor(x => x.Tin).NotEmpty().Matches("^\\d{9}$");

            RuleFor(x => x.Birthdate).Must(BeAValidDate).Must(NotBeFutureDate);

            RuleFor(x => x.TypeId).NotEmpty();
        }

        private bool BeAValidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

        private bool NotBeFutureDate(string value)
        {
            DateTime date;
            DateTime.TryParse(value, out date);
            return date < DateTime.Now;
        }
    }
}
