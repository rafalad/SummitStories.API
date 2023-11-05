using static System.Net.WebRequestMethods;

namespace SummitStories.API.Constants
{
    public enum AzureKeyVaultConfig
    {
        MailjetApiKey,
        MailjetApiSecret,
        BLOBConnectionString,
        BLOBContainerName,
        DBConnectionString,
        JWTValidIssuer,
        JWTValidAudience,
        JWTSecretKey
    }
}
