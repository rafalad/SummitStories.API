using Microsoft.OpenApi.Models;
using SummitStories.API.Constants;
using SummitStories.API.Email.EmailProvider;
using SummitStories.API.Email.HostedServices;
using SummitStories.API.Email.Interfaces;
using SummitStories.API.Modules.Blob.Interfaces;
using SummitStories.API.Modules.Blob.Repositories;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Repositories;
using SummitStories.API.Modules.SqlDb.Interfaces;
using SummitStories.API.Modules.SqlDb.Services;

namespace SummitStories.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Inne konfiguracje usług

            services.AddSingleton<EmailHostedService>();
            services.AddHostedService(provider => provider.GetService<EmailHostedService>());

            services.AddControllers(); // Dodaj kontrolery

            //services.AddMailService(provider =>
            //{
            //    string MailjetApiKey = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiKey)) ?? "";
            //    string MailjetApiSecret = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiSecret)) ?? "";
            //    return new(MailjetApiKey, MailjetApiSecret);
            //});

            //services.AddScoped<IEmailSender, EmailSender>(provider =>
            //{
            //    string mailjetApiKey = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiKey)) ?? "";
            //    string mailjetApiSecret = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiSecret)) ?? "";
            //    return new(mailjetApiKey, mailjetApiSecret);
            //});

            services.AddScoped<ISqlDbService, SqlDbService>(provider =>
            {
                string dbConnectionString = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.DBConnectionString)) ?? "";
                return new(dbConnectionString);
            });

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEmailSender, MailJetProvider>();

            services.AddScoped<IBlobStorageRepository, BlobStorageRepository>(provider =>
            {
                ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>()!;
                ILogger<BlobStorageRepository> logger = loggerFactory.CreateLogger<BlobStorageRepository>();
                string blobConnectionString = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.BLOBConnectionString)) ?? "";
                return new(logger, null, blobConnectionString);
            });

            // Konfiguracja Swagger
            if (Env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "SummitStories API",
                        Version = "v1"
                    });
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Konfiguracja Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Summit Stories Backend API v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            // Inne konfiguracje middleware, takie jak HTTPS, jeśli są potrzebne

            app.UseHttpsRedirection();

            // Mapowanie ścieżki na akcję kontrolera
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
