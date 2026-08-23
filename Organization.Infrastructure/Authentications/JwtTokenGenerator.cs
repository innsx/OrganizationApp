using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Organization.Application.Commons.ApplicationConfigOptions;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Authentications;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ValidUserResponseDto validUserResponseDto)
        {
            //we enter the claims that we want to include in the token,
            //such as the user's ID and email.
            //These claims will be used to identify the user when they make requests to our API.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, validUserResponseDto.Id),
                new Claim(JwtRegisteredClaimNames.Email, validUserResponseDto.Email)
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
                                DateTime.UtcNow.AddHours(1), //token VALID for 1 hour then expire
                                signingCredentials
            );

            // Finally, we return the token as a string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
