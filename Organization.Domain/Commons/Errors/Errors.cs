using ErrorOr;
using Organization.Domain.Commons.Errors.CustomsEnums;

namespace Organization.Domain.Commons.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            // Custom Error for Incorrect Email or Password
            public static Error IncorrectEmailOrPassword(string msg) =>
                Error.Validation(code: "EMAIL_PASSWORD_VALIDATION_ERR", description: msg ?? "Email or Password is Incorrect");

            // Custom Error for Invalid Refresh Token
            public static Error InvalidRefreshToken(string msg) =>
                Error.Custom(Convert.ToInt32(CustomEnumWithErrorTypes.UnAuthorized), "REFRESH_TOKEN_VALIDATION_ERR", msg ?? "Refresh token is invalid");

            // Custom Error for Expired Refresh Token
            public static Error RefreshTokenExpired(string msg) =>
                Error.Custom(Convert.ToInt32(CustomEnumWithErrorTypes.UnAuthorized), "REFRESH_TOKEN_EXPIRED_ERR", msg ?? "Refresh token has expired");

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