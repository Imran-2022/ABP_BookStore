using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Acme.BookStore.Data;
using Serilog;
using Volo.Abp;
using Volo.Abp.Data;

namespace Acme.BookStore.DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _configuration;

    public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var application = await AbpApplicationFactory.CreateAsync<BookStoreDbMigratorModule>(options =>
        {
           options.Services.ReplaceConfiguration(_configuration);
           options.UseAutofac();
           options.Services.AddLogging(c => c.AddSerilog());
           options.AddDataMigrationEnvironment();
        }))
        {
            await application.InitializeAsync();

            await application
                .ServiceProvider
                .GetRequiredService<BookStoreDbMigrationService>()
                .MigrateAsync();

            await application.ShutdownAsync();

            _hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}



/*

admin@abp.io
1q2w3E*  [when frontend---------------> aitai login krr jonno use hbe.]
1q2w3e* [others time]





dbmigrator -> [appsettings.json]


/*
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=BookStore;User ID=postgres;Password=pg2025;"
  },
  "OpenIddict": {
    "Applications": {
      "BookStore_App": {
        "ClientId": "BookStore_App",
        "RootUrl": "http://localhost:4200",
        "RedirectUris": "http://localhost:4200,http://localhost:4200/",
        "PostLogoutRedirectUris": "http://localhost:4200,http://localhost:4200/"
      },
      "BookStore_Swagger": {
        "ClientId": "BookStore_Swagger",
        "ClientSecret": "1q2w3e*",
        "RootUrl": "https://localhost:44348",
        "RedirectUris": "https://localhost:44348/swagger/oauth2-redirect.html",
        "PostLogoutRedirectUris": "https://localhost:44348/swagger"
      }
    }
  }
}


host appsettings.json

{
  "App": {
    "SelfUrl": "https://localhost:44348",
    "AngularUrl": "http://localhost:4200",
    "CorsOrigins": "https://*.BookStore.com,http://localhost:4200",
    "RedirectAllowedUrls": "http://localhost:4200",
    "DisablePII": false,
    "HealthCheckUrl": "/health-status"
  },
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=BookStore;User ID=postgres;Password=pg2025;"
  },
  "AuthServer": {
    "Authority": "https://localhost:44348",
    "RequireHttpsMetadata": true,
    "SwaggerClientId": "BookStore_Swagger",
    "CertificatePassPhrase": "b4a9b876-638f-4af6-b04f-c6e54d30bce0"
  },
  "OpenIddict": {
    "Applications": {
      "BookStore_App": {
        "ClientId": "BookStore_App",
        "RootUrl": "http://localhost:4200",
        "RedirectUris": "http://localhost:4200,http://localhost:4200/",
        "PostLogoutRedirectUris": "http://localhost:4200,http://localhost:4200/"
      },
      "BookStore_Swagger": {
        "ClientId": "BookStore_Swagger",
        "ClientSecret": "1q2w3e*",
        "RootUrl": "https://localhost:44348",
        "RedirectUris": "https://localhost:44348/swagger/oauth2-redirect.html",
        "PostLogoutRedirectUris": "https://localhost:44348/swagger"
      }
    }
  },
  "StringEncryption": {
    "DefaultPassPhrase": "J1h8MtabEWzie1TZ"
  }}
*/






/*


SELECT * FROM "AbpUsers" WHERE "UserName" = 'imran';
SELECT * FROM "AbpRoles" WHERE "Name" = 'admin';

UPDATE "AbpUsers"
SET "PasswordHash" = 'AQAAAAEAACcQAAAAEEdU1sG6N9yQvZ3X1s7Zp6Hk9t+WlFkN6xMZp7N+4vR6Z3Pvj8tYxT2eH7hN/8jLw==',
    "EmailConfirmed" = TRUE
WHERE "UserName" = 'admin';
UPDATE "AbpUsers"
SET 
    "PasswordHash" = 'AQAAAAEAACcQAAAAELaJfckKq/eWf7rFZl+9v6u8z/yQxQ1G+zOQlJ3uYl0vHqNHFQ0xT+F7/YmJd0wXHA==',
    "ConcurrencyStamp" = gen_random_uuid()::text,
    "SecurityStamp" = gen_random_uuid()::text
WHERE "UserName" = 'admin';


SELECT "Id", "UserName" FROM "AbpUsers";

INSERT INTO "AbpUserRoles" ("UserId", "RoleId")
VALUES (gen_random_uuid(), '3a1def9e-f0b6-0dcf-cd2f-fec1388ee5f0', '3a1df178-ab4f-6884-b31d-dbd098aecf98');




// bellow code work -> it replace the admin email pass, and make imran admin -> 
SELECT "Id", "Name"
FROM "AbpRoles";

SELECT "Id", "UserName", "Email"
FROM "AbpUsers";

INSERT INTO "AbpUserRoles" ("UserId", "RoleId")
VALUES ('3a1df178-ab4f-6884-b31d-dbd098aecf98', '3a1def9e-f2a8-8e9a-c328-684f5c685b3e');



*/



/*
admin ->
password -> imran.admin
username -> imran

username -> admin
admin pass -> 1q2w3E* 

user -> 
password -> user1.user
username -> user1

*/