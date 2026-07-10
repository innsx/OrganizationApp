using FluentValidation;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    //abstract class AbstractValidator<T> is part of the FluentValidation library,
    //which is a popular .NET library for building strongly-typed validation rules for objects.
    public class AddEmployeeCommandValidator : AbstractValidator<AddEmployeeCommand>
    {
        public AddEmployeeCommandValidator()
        {
            RuleFor(e => e.Name)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("NAME_VALIDATION_ERR_MESSAGE")
                .WithMessage("Employee Name is mandatory.");
                         
            RuleFor(e => e.Age)
                .GreaterThanOrEqualTo(18)
                .WithErrorCode("AGE_LESS_18_VALIDATION_ERR_MESSAGE")
                .WithMessage("Employee Age must be between 18-50");

            RuleFor(e => e.Age)
                .LessThanOrEqualTo(50)
                .WithErrorCode("AGE_GREATER_50_VALIDATION_ERR_MESSAGE")
                .WithMessage("Employee Age must be between 18-50");

            RuleFor(e => e.Salary)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("SALARY_VALIDATION_ERR_MESSAGE")
                .WithMessage("Salary can not have null/empty value.");


            RuleFor(e => e.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("COMPANY_ID_VALIDATION_ERR_MESSAGE")
                .WithMessage("Foreign Key CompanyId can not be Null/Empty.");

        }
    }
}
