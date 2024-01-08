using static System.Net.WebRequestMethods;

namespace SummitStories.API.Constants
{
    public enum AzureKeyVaultConfig
    {
        BLOBConnectionString,
        BLOBContainerName,
        DBConnectionString,
        JWTValidIssuer,
        JWTValidAudience,
        JWTSecretKey,
        MailjetApiKey,
        MailjetApiSecret,
        MailjetApiEmail
    }
}
