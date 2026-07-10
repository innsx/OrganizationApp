using FluentValidation;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public class AddCompanyCommandValidator : AbstractValidator<AddCompanyCommand>
    {
        public AddCompanyCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithErrorCode("COMPANY_NAME_VALIDATION_ERROR")
                .WithMessage("Company Name Field can not be Empty.");

            RuleFor(c => c.Name)
                .NotNull()
                .WithErrorCode("COMPANY_NAME_VALIDATION_ERROR")
                .WithMessage("Company Name Field can not be Null.");

            RuleFor(c => c.Address)
                .NotNull()
                .WithErrorCode("COMPANY_ADDRESS_VALIDATION_ERROR")
                .WithMessage("Company Adress Field can not be Null.");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithErrorCode("COMPANY_ADDRESS_VALIDATION_ERROR")
                .WithMessage("Company Address Field can not be Empty.");

            RuleFor(c => c.Address)
                .MaximumLength(50)
                .WithErrorCode("COMPANY_ADDRESS_LENGTH_VALIDATION_ERROR")
                .WithMessage("Company Address Field can not have more than 50 characters.");

            RuleFor(c => c.Country)
                .MaximumLength(50)
                .WithErrorCode("COMPANY_COUNTRY_LENGTH_VALIDATION_ERROR")
                .WithMessage("Company Country Field can not have more than 50 characters.");

            RuleFor(c => c.Country)
                .NotNull()
                .WithErrorCode("COMPANY_COUNTRY_VALIDATION_ERROR")
                .WithMessage("Company Country Field can not be Null.");

            RuleFor(c => c.Country)
                .NotEmpty()
                .WithErrorCode("COMPANY_COUNTRY_VALIDATION_ERROR")
                .WithMessage("Company Country Field can not be Empty.");

        }
    }
}