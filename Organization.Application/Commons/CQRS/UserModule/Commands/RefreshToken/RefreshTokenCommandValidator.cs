using FluentValidation;
using Organization.Application.Commons.DTOs;
using Organization.Domain.Commons.Utilities;

namespace Organization.Application.Commons.CQRS.UserModule.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommandDto>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email cannot be null.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");


            RuleFor(r => r.Email).Custom((email, context) => {
                if (!email.IsValidEmail())
                {
                    context.AddFailure("EMAIL_VALIDATION_ERROR", "Invalid user email.");
                }
            });

        }
    }
}
