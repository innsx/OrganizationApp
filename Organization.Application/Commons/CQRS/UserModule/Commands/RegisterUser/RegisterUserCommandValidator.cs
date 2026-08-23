using FluentValidation;
using Organization.Domain.Commons.Utilities;

namespace Organization.Application.Commons.CQRS.UserModule.Commands.RegisterUser
{
    //AbstractValidator<RegisterUserCommand> core syntax for
    //  creating a validator using the popular .NET library FluentValidation.
    //It strongly types the validator to a specific command,
    //  typically used in the CQRS (Command Query Responsibility Segregation) pattern.
    public sealed class RegisterUserCommandValidator :  AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() 
        {
            RuleFor(rc => rc.registerUserRequestDto.UserName)
                .NotEmpty()
                .NotNull()
                .WithErrorCode("VALUE_ERROR_USR000")
                .WithMessage("UserName is required.");

            RuleFor(rc => rc.registerUserRequestDto.UserName)
                .Must(x => x.Length >= 8)
                .WithErrorCode("VALUE_ERROR_USR000")
                .WithMessage("UserName length must be greater than or equal to 8 characters.");


            RuleFor(rc => rc.registerUserRequestDto.Email)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("VALUE_ERROR_USR001")
                .WithMessage("User email is required.");


            RuleFor(rc => rc.registerUserRequestDto.Email).Custom((email, context) => {
                if (!email.IsValidEmail())
                {
                    context.AddFailure("VALUE_ERROR_USR002", "Invalid email.");
                }
            });


            RuleFor(rc => rc.registerUserRequestDto.Password)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("VALUE_ERROR_USR003")
                .WithMessage("Password is required.");


            //error message NOT SPECIFIC enough
            //RuleFor(rc => rc.registerUserRequestDto.Password).Custom((password, context) => {
            //    if (!password.IsValidPassword())
            //    {
            //        context.AddFailure("VALUE_ERROR_USR004", "Invalid Password.");
            //    }
            //});

            //this gives SPECIFIC ERROR MESSAGE RELATED TO THE ERROR
            RuleFor(x => x.registerUserRequestDto.Password)
            .MinimumLength(12)
                .WithMessage("Password must be at least 12 characters")
            .MaximumLength(20)
                .WithMessage("Password cannot exceed 20 characters")
            .Matches("[A-Z]")
                .WithMessage("Password must contain at least one capital letter.")
            .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain a special character.");

        }
    }
}