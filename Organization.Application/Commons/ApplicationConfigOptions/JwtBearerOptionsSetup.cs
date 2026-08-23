using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Organization.Application.Commons.ApplicationConfigOptions
{
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;

        // Constructor to initialize JwtBearerOptionsSetup with JwtOptions using IOptions<JwtOptions>
        public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            // Configure the JwtBearerOptions using the values from _jwtOptions
            //options.Authority = _jwtOptions.Authority;
            //options.Audience = _jwtOptions.Audience;
            //options.RequireHttpsMetadata = _jwtOptions.RequireHttpsMetadata;

            // Set to false FOR DEVELOPMENT purposes; OR set to true FOR PRODUCTION
            options.RequireHttpsMetadata = false; 

            // Save the token in the AuthenticationProperties after a successful authorization
            options.SaveToken = true; 

            // Set the TokenValidationParameters based on the JwtOptions
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Validate the issuer of the token
                ValidateAudience = true, // Validate the audience of the token
                ValidateLifetime = true, // Validate the expiration and not before values in the token
                ValidateIssuerSigningKey = true, // Validate the issuer signing key
                ValidIssuer = _jwtOptions.Issuer, // Set the valid issuer from JwtOptions
                ValidAudience = _jwtOptions.Audience, // Set the valid audience from JwtOptions
                // Set the issuer signing key using the secret key from JwtOptions
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)) 
            };
        }
    }
}
