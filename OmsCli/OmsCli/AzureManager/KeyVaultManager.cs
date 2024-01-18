using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Configuration;

namespace OmsCli.AdfManager
{
    public class KeyVaultManager
    {
        public static string GetSecret(string secretName)
        {
            Settings setting = new();

            string keyVaultUri = setting.KeyVaultUrl;
            string clientId = setting.ApplicationId;
            string clientSecret = setting.ApplicationSecret;
            string tenantId = setting.TenantId;

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var secretClient = new SecretClient(new Uri(keyVaultUri), clientSecretCredential);

            try
            {
                KeyVaultSecret secret = secretClient.GetSecret(secretName);
                return secret.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing Key Vault: {ex.Message}");
                return "Could not retrieve credentials";
            }
        }
    }
}