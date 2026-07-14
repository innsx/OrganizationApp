using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Organization.Application.Commons.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Commons.ApplicationConfigOptions
{
    public class OrganizationOptionsSetup : IConfigureOptions<OrganizationOption>
    {
        private readonly IConfiguration _configuration;
        public OrganizationOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(OrganizationOption options)
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
            _configuration.GetSection(GlobalConstants.ConfigurationSections.OrganizationApp).Bind(options);

        }
    }
}
