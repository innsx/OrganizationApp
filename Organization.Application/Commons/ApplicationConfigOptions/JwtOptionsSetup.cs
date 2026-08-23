using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Organization.Application.Commons.Utilities;

namespace Organization.Application.Commons.ApplicationConfigOptions
{
    public class JwtOptionsSetup: IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(GlobalConstants.ConfigurationSections.Jwt).Bind(options);
        }
    }
}



//options.Issuer = "https://localHost:7160";
//options.Audience = "https://localHost:7160";
//options.SecretKey = "D65D6DAC-EF51F-4486-AD29-FB49A8CDD215";