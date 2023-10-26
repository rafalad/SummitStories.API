using Azure.Identity;
using Microsoft.AspNetCore;
using SummitStories.API.Constants;

namespace SummitStories.API;

public class Program
{
    public static void Main(string[] args)
    {
        IWebHost webHost = CreateWebHostBuilder(args).Build();
        webHost.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                //string keyVaultUrl = AppServiceSettingsConfig.KeyVaultUri;
                //if (!string.IsNullOrEmpty(keyVaultUrl))
                //{
                //    config.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
                //}

                string environment = AppServiceSettingsConfig.Env.ToLowerInvariant() ?? "production";

                config.SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .UseStartup<Startup>();
}
