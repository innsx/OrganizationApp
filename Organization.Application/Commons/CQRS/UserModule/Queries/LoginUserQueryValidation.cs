using FluentValidation;
using Organization.Domain.Commons.Utilities;

namespace Organization.Application.Commons.CQRS.UserModule.Queries
{
    public class LoginUserQueryValidation : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidation() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .NotEmpty()
                    .WithErrorCode("EMAIL_VALIDATION_ERR")
                    .WithMessage("Email is required.")
                .EmailAddress()
                    .WithMessage("Invalid email format.");


            RuleFor(x => x.Email).Custom((email, context) =>
            {
                if (!string.IsNullOrEmpty(email) && !email.EndsWith("@example.com") && !email.IsValidEmail())
                {
                    context.AddFailure("EMAIL_VALIDATION_ERR", "Email must end with @example.com.");
                }
            });


            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .NotEmpty()
                    .WithErrorCode("PASSWORD_VALIDATION_ERR")
                    .WithMessage("Password is required.")
                .MinimumLength(8)
                    .WithErrorCode("PASSWORD_VALIDATION_ERR")
                    .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]+")
                    .WithErrorCode("PASSWORD_VALIDATION_ERR")
                    .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+")
                    .WithErrorCode("PASSWORD_VALIDATION_ERR")
                    .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[!@#$%^&*]")  //(@"[\!\?\*\.]+")
                    .WithErrorCode("PASSWORD_VALIDATION_ERR")
                    .WithMessage("Password must contain at least one special character (!@#$%^&*).");
                //.Matches(@"[0-9]+")
                //    .WithErrorCode("PASSWORD_VALIDATION_ERR")
                //    .WithMessage("Password must contain at least one digit.")
        }
    }
} 
