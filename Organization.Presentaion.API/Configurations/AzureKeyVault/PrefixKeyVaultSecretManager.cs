using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace Organization.Presentaion.API.Configurations.AzureKeyVault
{
    public sealed class PrefixKeyVaultSecretManager : KeyVaultSecretManager
    {
        private readonly string _prefix;

        public PrefixKeyVaultSecretManager(string prefix)
        {
            _prefix = $"{prefix}-";
        }

        public override string GetKey(KeyVaultSecret secret)
        {
            return secret.Name.Substring(_prefix.Length).Replace("--", ConfigurationPath.KeyDelimiter);
        }

        public override bool Load(SecretProperties properties)
        {
            return properties.Name.StartsWith(_prefix);
        }
    }

}
