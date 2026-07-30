using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Organization.Application.Commons.Utilities;

namespace Organization.Application.Commons.ApplicationConfigOptions
{
    //In .NET, IConfigureOptions<OrganizationOption> is a fundamental interface used
    //within the Options Pattern to dynamically configure
    //an instance of your custom OrganizationOption class.
    //It is primarily implemented when you need to resolve external dependencies
    //from the Dependency Injection (DI) container—such as databases, HTTP clients,
    //or other services—to determine your configuration values.

    //Step 1: define the Options Class "OrganizationOption":
    //This class represents the strongly-typed schema of your organization configuration data.

    //Step 2. Implement IConfigureOptions<OrganizationOption>
    //Create a dedicated class that implements the interface.
    //The DI container injects any necessary external services directly into its constructor.
    public class OrganizationOptionsSetup : IConfigureOptions<OrganizationOption>
    {
        private readonly IConfiguration _configuration;

        //we injecting IConfiguration in the constructor
        public OrganizationOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(OrganizationOption organizationOptions)
        {
            //here we're HARD-CODED "OrganizationAppSection" section in AppSettings.json file
            //& then binding ALL Appsettings.json's "OrganizationAppSection" Section's settings
            // to this "options" object
            // instead HARD-CODED "strings" all over our Application
            //like below statement; so we commented it  
            //_configuration.GetSection("OrganizationAppSection").Bind(options);

            // ....and use GetSection & ACCESS OrganizationAppSection Key thru
            // a GLOBAL CONSTANT variable we named it "OrganizationApp"
            //we're DYNAMICALLY calling Appsettings.json's Section "OrganizationAppSection"
            // settings during RUNTIME & bind the OrganizationAppSection values to an “options” object
            // of TYPED OrganizationOption
            _configuration.GetSection(GlobalConstants.ConfigurationSections.OrganizationAppSection).Bind(organizationOptions);

        }
    }
}
