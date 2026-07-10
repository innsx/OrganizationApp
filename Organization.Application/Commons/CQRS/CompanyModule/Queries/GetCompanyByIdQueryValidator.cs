using FluentValidation;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    public class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
    {
        public GetCompanyByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithErrorCode("COMPANY_ID_VALIDATION_ERR")
                .WithMessage("Company Id can not be Empty.");

            RuleFor(c => c.Id)
                .NotNull()
                .WithErrorCode("COMPANY_ID_VALIDATION_ERR")
                .WithMessage("Company Id can not be Null.");

            RuleFor(c => c.Id)
                .NotEqual(22.ToString())
                .WithErrorCode("COMPANY_ID_VALIDATION_ERR")
                .WithMessage("Company Id must have 22 Guid characters.");

        }
    }
}
