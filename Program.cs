using Mailjet.Client;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using SummitStories.Api.Modules.Data;
using SummitStories.Api.Modules.Data.Interfaces;
using SummitStories.Api.Modules.Data.Repositories;
using SummitStories.Api.Modules.Data.Services;
using SummitStories.API.Modules.Blob.Interfaces;
using SummitStories.API.Modules.Blob.Repositories;
using SummitStories.API.Modules.Email;
using SummitStories.API.Modules.Email.Interfaces;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();


builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddControllers();


builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
{
    client.SetDefaultSettings();
    client.UseBasicAuthentication(
        builder.Configuration.GetConnectionString("MailjetApiKey") ?? "",
        builder.Configuration.GetConnectionString("MailjetApiSecret") ?? "");
});

builder.Services.AddScoped<IBlobStorageRepository, BlobStorageRepository>(provider =>
{
    ILoggerFactory loggerFactory = provider.GetService<ILoggerFactory>()!;
    ILogger<BlobStorageRepository> logger = loggerFactory.CreateLogger<BlobStorageRepository>();
    string blobConnectionString = builder.Configuration.GetConnectionString("BLOBConnectionString") ?? "";
    return new(logger, null, blobConnectionString);
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    });
}

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "https://localhost:7271",
                "https://summitstories.blog",
                "https://www.summitstories.blog")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// WebSocket support
builder.Services.AddWebSockets(options =>
{
    options.KeepAliveInterval = TimeSpan.FromMinutes(2);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseWebSockets();

app.UseMiddleware<SummitStories.Api.Middleware.WebSocketManager>();

app.MapControllers();

app.Run();
