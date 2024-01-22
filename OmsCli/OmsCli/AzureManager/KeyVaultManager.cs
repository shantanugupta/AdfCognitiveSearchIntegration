using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

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
                throw new ApplicationException($"Error retriving secret from key vault. Error message: {ex.Message}", ex);
            }
        }
    }
}