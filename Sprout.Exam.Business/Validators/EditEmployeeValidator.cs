using FluentValidation;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Validators
{
    public class EditEmployeeValidator : AbstractValidator<EditEmployeeDto>
    {
        public EditEmployeeValidator(IValidator<BaseSaveEmployeeDto> baseValidator)
        {
            RuleFor(x => x).SetValidator(baseValidator);

            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
