using Azure.Identity;
using Serilog;
using System.Security.Cryptography.X509Certificates;

namespace Organization.Presentaion.API.Configurations.AzureKeyVault
{
    public static class AzureKeyVaultConfiguration
    {
        public static WebApplicationBuilder ConfigureAzureKeyVault(this WebApplicationBuilder builder)
        {
            //reads AzureKeyVault section from AppSettings.json which connects to Azure Key Vault key/value pair
            var storeName = builder.Configuration["AzureKeyVault:KeyVaultCertStoreName"] == "Personal" ? "My" : builder.Configuration["AzureKeyVault:KeyVaultCertStoreName"];

            using var certStorageLocation = builder.Configuration["AzureKeyVault:KeyVaultCertStoreLocation"] == "LocalMachine" ?
                              new X509Store(storeName, StoreLocation.LocalMachine) : new X509Store(StoreLocation.CurrentUser);

            certStorageLocation.Open(OpenFlags.ReadOnly);

            var certificate = certStorageLocation.Certificates.Find(X509FindType.FindByThumbprint, builder.Configuration["AzureKeyVault:KeyVaultCertThumbPrint"], false);

            if (certificate.OfType<X509Certificate2>().Count() == 0)
            {
                Log.Error("KeyVault certificate for the specified thumbprint not found.");
            }
            else if (certificate.OfType<X509Certificate2>().Count() > 1)
            {
                Log.Error("Multiple certificates found.");
            }

            builder.Configuration.AddAzureKeyVault(
                     new Uri(builder.Configuration["AzureKeyVault:KeyVaultBaseUrl"]),
                     new ClientCertificateCredential(
                         builder.Configuration["AzureKeyVault:AzureAppRegDirectoryId"],
                         builder.Configuration["AzureKeyVault:AzureAppRegApplicationId"],
                         certificate.OfType<X509Certificate2>().Single()),
                     new PrefixKeyVaultSecretManager(builder.Configuration["AzureKeyVault:KeyVaultSettingsPrefix"])
            );

            certStorageLocation.Close();

            return builder;
        }
    }

}
