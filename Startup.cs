using Mailjet.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SummitStories.API.Auth;
using SummitStories.API.Constants;
using SummitStories.API.Modules.Blob.Interfaces;
using SummitStories.API.Modules.Blob.Repositories;
using SummitStories.API.Modules.Data.Interfaces;
using SummitStories.API.Modules.Data.Repositories;
using SummitStories.API.Modules.Email;
using SummitStories.API.Modules.Email.Interfaces;
using SummitStories.API.Modules.SqlDb.Interfaces;
using SummitStories.API.Modules.SqlDb.Services;
using System.Text;

namespace SummitStories.API;

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
        services.AddScoped<ISqlDbService, SqlDbService>(provider =>
        {
            string dbConnectionString = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.DBConnectionString)) ?? "";
            return new(dbConnectionString);
        });

        services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.DBConnectionString))));

        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
         {
             string validAudience = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.JWTValidAudience)) ?? "";
             string validIssuer = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.JWTValidIssuer)) ?? "";
             string JWTSecretKey = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.JWTSecretKey)) ?? "";

             options.SaveToken = true;
             options.RequireHttpsMetadata = false;
             options.TokenValidationParameters = new TokenValidationParameters()
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidAudience = validAudience,
                 ValidIssuer = validIssuer,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecretKey))
             };
         });

        services.AddLogging(builder =>
        {
            builder.AddConsole();
                                 
        });

        services.AddControllers();

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddRazorPages();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
        services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
        {
            client.SetDefaultSettings();
            client.UseBasicAuthentication(
                Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiKey)) ?? "",
                Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.MailjetApiSecret)) ?? "");
        });

        //services.AddScoped<IStravaRepository, StravaRepository>(provider =>
        //{
        //    ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>()!;
        //    ILogger<StravaRepository> logger = loggerFactory.CreateLogger<StravaRepository>();
        //    string StravaAccessToken = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.StravaAccessToken)) ?? "";
        //    return new(logger, null, StravaAccessToken);
        //});

        services.AddScoped<IBlobStorageRepository, BlobStorageRepository>(provider =>
        {
            ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>()!;
            ILogger<BlobStorageRepository> logger = loggerFactory.CreateLogger<BlobStorageRepository>();
            string blobConnectionString = Configuration.GetValue<string>(nameof(AzureKeyVaultConfig.BLOBConnectionString)) ?? "";
            return new(logger, null, blobConnectionString);
        });

        if (Env.IsDevelopment())
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SummitStories.blog API",
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SummitStories.blog API v1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHttpsRedirection();
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
        });
    }
}