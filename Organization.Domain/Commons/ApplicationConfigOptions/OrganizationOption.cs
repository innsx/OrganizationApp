namespace Organization.Application.Commons.ApplicationConfigOptions
{

    //Step 1: define the Options Class:
    //This represents the strongly-typed schema of your organization configuration data.
    public sealed class OrganizationOption
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ConfidentialData { get; set; }
    }
}
