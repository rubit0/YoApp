using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace YoApp.Chat.Core
{
    public static class OwinExtensions
    {
        /// <summary>
        /// **Preview for ASP.NET Core 2.0**
        /// Configures data protection (AspNetCore 2.0) to use the azure blob storage.
        /// </summary>
        /// <param name="services">Target service instance.</param>
        /// <param name="applicationName">Must be same accross all Services</param>
        /// <param name="accountName">Account name of the blob storage</param>
        /// <param name="accountKey">Key of the account</param>
        /// <param name="containerName">Blob target container.</param>
        public static void ConfigureDataProtectionForAspNet2OnAzure(this IServiceCollection services,
            string applicationName, string accountName, string accountKey, string containerName = "ringkeys")
        {
            var credentials = new StorageCredentials(accountName, accountKey);

            var account = new CloudStorageAccount(credentials, true);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);
            container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            //Set App wide protection keyring
            services.AddDataProtection()
                .SetApplicationName(applicationName)
                .PersistKeysToAzureBlobStorage(container, "keys.xml");
        }
    }
}
