using FluentValidation;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator()
        {
            RuleFor(e => e.id)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("EMP_ID_VALIDATION_ERR_MESSAGE")
                .WithMessage("Employee Id can not be null or empty.");


            //RuleFor(e => e.isDeleteHasAssociations)
            //    .NotNull() 
            //    .NotEmpty()
            //    .WithErrorCode("EMP_isDeleteAssociations_VALIDATION_ERR_MESSAGE")
            //    .WithMessage("isDeleteAssociations value can not be null/empty.");
        }
    }
}

