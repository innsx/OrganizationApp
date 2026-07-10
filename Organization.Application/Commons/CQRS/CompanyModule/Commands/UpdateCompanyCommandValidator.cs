using FluentValidation;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id cannot be null.")
                .NotEmpty().WithMessage("Id is required.")
                .MaximumLength(22).WithMessage("Id must not exceed 22 characters.");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address cannot be null.")
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

            RuleFor(x => x.Country)
                .NotNull().WithMessage("Country cannot be null.")
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");
        }
    }
}
