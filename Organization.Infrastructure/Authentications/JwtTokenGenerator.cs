using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Organization.Application.Commons.ApplicationConfigOptions;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Authentications;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Users.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Organization.Infrastructure.Authentications
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        //The JwtTokenGenerator class is responsible for generating JSON Web Tokens (JWTs)
        //for authenticated users.
        //It implements the IJwtTokenGenerator interface,
        //which defines the contract for generating tokens.
        private readonly JwtOptions _jwtOptions;

        //The IHttpContextAccessor is used to access the current HTTP context,
        private readonly IHttpContextAccessor _httpContextAccessor;

        //The IUnitOfWork is used to manage database transactions and access the User entity.
        private readonly IUnitOfWork _unitOfWork;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _jwtOptions = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(User user)
        {
            //we enter the claims that we want to include in the token,
            //such as the user's ID and email.
            //These claims will be used to identify the user when they make requests to our API.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            //we create a signing key using the secret key from our configuration.
            var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), 
                                                                     SecurityAlgorithms.HmacSha256Signature
            );

            //we create a new JWT token using the claims, jwtOptions.Issurer, jwtOptions.Audience
            //and signing credentials we created above and an expiration time.
            var token = new JwtSecurityToken(
                                _jwtOptions.Issuer,
                                _jwtOptions.Audience,
                                claims,
                                null,
                                //DateTime.UtcNow.AddHours(1), //token VALID for 1 hour then expire
                                DateTime.UtcNow.AddMinutes(1), //token VALID for 1 minute then expire
                                signingCredentials
            );

            // Finally, we return the token as a string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenDto GenerateRefreshToken()
        {
            // The GenerateRefreshToken method generates a token
            // and assign it to the TokenValue property
            // & generate a expires date and assign Expireof the RefreshTokenDto.
            return new RefreshTokenDto
            {
                // We use the RandomNumberGenerator class from System.Security.Cryptography package
                // to generate a secure random byte array
                TokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                //Expires = DateTime.UtcNow.AddHours(7) // The refresh token is valid for 7 hours, after which it will expire.
                Expires = DateTime.UtcNow.AddMinutes(1) // The refresh token is valid for 1 minute, after which it will expire.
            };
        }

        public void SetRefreshTokenAsHttpOnlyCookie(RefreshTokenDto refreshTokenDto)
        {
            // The SetRefreshTokenAsHttpOnlyCookie method sets the refresh token as
            // an HTTP-only cookie in the user's browser.
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenDto.Expires,
            };

            // We use the IHttpContextAccessor to access the current HTTP context
            // and set the refresh token cookie.
            var httpContext = _httpContextAccessor.HttpContext;

            // Append the refresh token to the response cookies with the specified options.
            httpContext?.Response.Cookies.Append(GlobalConstants.RefreshTokenCookieKey, refreshTokenDto.TokenValue, cookieOptions);
        }

        public async Task<string> DoTokenCreationAsync(User user)
        {
            // The DoTokenCreationAsync method generates a new access token and refresh token for the user,
            // sets the refresh token as an HTTP-only cookie, and updates the user's refresh token in the database.
            var accessToken = GenerateAccessToken(user);

            // Generate a new refresh token and set it as an HTTP-only cookie in the user's browser.
            var newRefreshToken = GenerateRefreshToken();

            // Set the refresh token as an HTTP-only cookie in the user's browser.
            SetRefreshTokenAsHttpOnlyCookie(newRefreshToken);

            // Open a database connection and begin a transaction to update the user's refresh token in the database.
            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            // Update the user's refresh token and expiry date in the database.
            user.RefreshToken = newRefreshToken.TokenValue;

            // Update the user's refresh token expiry date in the database.
            user.RefreshTokenExpiryDate = newRefreshToken.Expires;

            //  Update the user in the database asynchronously.
            await _unitOfWork.Users.UpdateAsync(user);

            // Commit the transaction, dispose of the transaction and close the database connection.
            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            // Finally, return the generated access token to the caller.
            return accessToken;
        }
    }
}





