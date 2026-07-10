using FluentValidation;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
    {
        public DeleteCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id cannot be null.")
                .NotEmpty().WithMessage("Id is required.")
                .MaximumLength(22).WithMessage("Id must not exceed 22 characters.");
        }
    }
}
