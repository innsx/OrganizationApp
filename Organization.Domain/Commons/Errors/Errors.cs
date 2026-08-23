using ErrorOr;

namespace Organization.Domain.Commons.Errors
{
    public static partial class Errors
    {
        public static class User
        {

            public static Error IncorrectEmailOrPassword(string msg) =>
                    Error.Validation(code: "EMAIL_PASSWORD_VALIDATION_ERR", description: msg ?? "Email or Password is Incorrect");

        }
    }
}







//public static Error NotFound(string userId) => new Error(
//    code: "User.NotFound",
//    description: $"User with ID '{userId}' was not found."
//);
//public static Error InvalidEmail(string email) => new Error(
//    code: "User.InvalidEmail",
//    description: $"The email address '{email}' is not valid."
//);
//public static Error PasswordTooWeak() => new Error(
//    code: "User.PasswordTooWeak",
//    description: "The provided password does not meet the security requirements."
//);