using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OneMore.Api.Hubs.ConnManager;
using OneMore.Domain.Handlers;
using OneMore.Domain.Repositories;
using OneMore.Domain.Services;
using OneMore.Domain.Services.Abstract;
using OneMore.Infra.Data;
using OneMore.Infra.Data.Repositories;
using System.Reflection;

namespace OneMore.API.Extentions;

public static class BuilderExtentions
{
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OneTime",
                Version = "v1",
                Description = $"<br />Data de compilação: <b>{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss}</b> UTC"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5c ...\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<SessionHandler, SessionHandler>();

        //builder.Services.AddSingleton<IConnectionMultiplexer>(
        //    ConnectionMultiplexer.Connect("localhost:6379")
        //);

        builder.Services.AddTransient<IConnectionManager, InMemoryConnectionManager>();
        builder.Services.AddTransient<IWordRepository, WordRepository>();
        builder.Services.AddTransient<ISessionService, SessionService>();

    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        try
        {
            builder.Services.AddDbContextFactory<DataContext>(db => db
                .UseMySql(
                    builder.Configuration.GetValue<string>("ConnectionStrings:connectionString"),
                    new MySqlServerVersion(builder.Configuration.GetValue<string>("ConnectionStrings:DbVersion")),
                    options => options
                        .EnableRetryOnFailure()
                        .MigrationsAssembly("OneMore.Infra")
                ));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on AddDatabase: {ex.Message}");
        }
    }

    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("SignalRCors", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            });
        });

    }
}