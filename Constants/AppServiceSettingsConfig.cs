namespace SummitStories.API.Constants
{
    public class AppServiceSettingsConfig
    {
        public static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
        public static readonly string KeyVaultUri = Environment.GetEnvironmentVariable("KEYVAULT_URI") ?? "";
    }
}
