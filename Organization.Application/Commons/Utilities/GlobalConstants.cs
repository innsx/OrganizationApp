namespace Organization.Application.Commons.Utilities
{
    public static class GlobalConstants
    {
        public const string Errors = "errors";
        public const string ApplicationName = "Organization.Presentation.API";
        public const string RefreshTokenCookieKey = "refreshToken";

        public static class ConfigurationSections
        {
            //creating a GLOBAL constant "OrganizationAppSection"
            //for Appsettings.json’s section "OrganizationAppSection"
            public const string OrganizationAppSection = "OrganizationAppSection";

            public const string Jwt = "Jwt";
            public const string My3rdpartyProductOptions = "My3rdpartyProductoptions";
        }

        public static class CustomClaims
        {
            public const string Permissions = "permissions";
        }
    }
}
