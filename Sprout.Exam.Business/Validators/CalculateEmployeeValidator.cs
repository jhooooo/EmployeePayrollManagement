using FluentValidation;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Validators
{
    public class CalculatePayrollValidator : AbstractValidator<CalculatePayrollDto>
    {
        public CalculatePayrollValidator()
        {
            RuleFor(x => x.Absences).Must(x => x % 0.5 == 0).WithMessage("Input is not a valid worked days value");
            RuleFor(x => x.Absences).LessThan(23).When(x => x.Absences != 0);
            RuleFor(x => x.Absences).GreaterThan(0).When(x => x.Absences != 0);

            RuleFor(x => x.DaysWorked).Must(x => x % 0.5 == 0).WithMessage("Input is not a valid worked days value");
            RuleFor(x => x.DaysWorked).LessThan(23).When(x => x.DaysWorked != 0);
            RuleFor(x => x.DaysWorked).GreaterThan(0).When(x => x.DaysWorked != 0);
        }
    }
}
