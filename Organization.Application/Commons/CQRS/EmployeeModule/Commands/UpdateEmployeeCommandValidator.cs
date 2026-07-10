using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .NotNull()
                .WithErrorCode("EMP_NAME_VALIDATION_ERROR")
                .WithMessage("Employee name is required.");


            RuleFor(e => e.Age)
                .NotEmpty()
                .NotNull()
                .WithErrorCode("EMP_AGE_VALIDATION_ERROR")
                .WithMessage("Employee age is required.");


            RuleFor(e => e.Age)
                .GreaterThanOrEqualTo(18)
                .WithErrorCode("EMP_AGE_VALIDATION_ERROR")
                .WithMessage("Employee age must be greater than or equal to 18.");

            RuleFor(e => e.Age)
                .LessThanOrEqualTo(50)
                .WithErrorCode("EMP_AGE_VALIDATION_ERROR")
                .WithMessage("Employee Age must be between 18-50.");

            RuleFor(e => e.Salary)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("EMP_SALARY_VALIDATION_ERROR")
                .WithMessage("Employee salary cannot be Null/Empty.");

            RuleFor(e => e.CompanyId)
                .NotEmpty()
                .NotNull()
                .WithErrorCode("EMP_COMPANY_ID_VALIDATION_ERROR")
                .WithMessage("Employee company ID is mandatory.");

            RuleFor(e => e.CompanyId)
                .MaximumLength(22)
                .WithErrorCode("EMP_COMPANY_ID_VALIDATION_ERROR")
                .WithMessage("Employee company ID must not exceed 22 characters.");
        }
    }
}
