using FluentValidation;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator(IValidator<BaseSaveEmployeeDto> baseValidator)
        {
            RuleFor(x => x).SetValidator(baseValidator);
        }
    }
}
