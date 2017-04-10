using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace YoApp.Utils.Extensions
{
    public static class OwinExtensions
    {
        /// <summary>
        /// Configures data protection to use azure blob storage.
        /// </summary>
        /// <param name="services">Target service instance.</param>
        /// <param name="applicationName">Must be same accross all Services</param>
        /// <param name="accountName">Account name of the blob storage</param>
        /// <param name="accountKey">Key of the account</param>
        /// <param name="containerName">Blob target container.</param>
        public static void ConfigureDataProtectionOnAzure(this IServiceCollection services, 
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

        /// <summary>
        /// Configures data protection to use local storage.
        /// </summary>
        /// <param name="services">Target service instance.</param>
        /// <param name="applicationName">Must be same accross all Services</param>
        public static void ConfigureDataProtectionLocal(this IServiceCollection services, string applicationName)
        {
            services.AddDataProtection().SetApplicationName(applicationName);
        }
    }
}
