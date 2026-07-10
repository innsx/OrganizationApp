using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Queries
{
    public class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
    {
        public GetEmployeeByIdQueryValidator() 
        {
            RuleFor(e => e.id)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("ID_VALIDATION_ERR_MESSAGE")
                .WithMessage("Employee ID is mandatory.");

            RuleFor(e => e.id)
                .MaximumLength(22)
                .WithErrorCode("EMP_ID_VALIDATION_ERROR")
                .WithMessage("Employee ID must not exceed 22 characters.");

            RuleFor(e => e.id)
                .MinimumLength(22)
                .WithErrorCode("EMP_ID_VALIDATION_ERROR")
                .WithMessage("Employee ID must be exactly 22 characters.");
        }
    }
}
