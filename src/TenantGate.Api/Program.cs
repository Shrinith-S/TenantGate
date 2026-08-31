using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TenantGate.Application;
using TenantGate.Application.Services;
using TenantGate.Infrastructure;
using TenantGate.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();


// ===============================
// Database
// ===============================

builder.Services.AddDbContext<TenantGateDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TenantGateDb"));
});


// ===============================
// Application Services
// ===============================

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();


// ===============================
// Azure AD / JWT Authentication
// ===============================

var tenantId = builder.Configuration["AzureAd:TenantId"];
var audience = builder.Configuration["AzureAd:Audience"];

Console.WriteLine("========== Azure AD Configuration ==========");
Console.WriteLine($"TenantId: {tenantId}");
Console.WriteLine($"Audience: {audience}");
Console.WriteLine("============================================");


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            $"https://login.microsoftonline.com/{tenantId}";

        options.Audience = audience;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidAudience = audience,

                AudienceValidator =
                    (tokenAudiences, securityToken, validationParameters) =>
                    {
                        Console.WriteLine();
                        Console.WriteLine(
                            "========== AUDIENCE DEBUG =========="
                        );

                        Console.WriteLine(
                            $"Expected audience: {audience}"
                        );

                        Console.WriteLine(
                            $"Token audiences: " +
                            $"{string.Join(", ", tokenAudiences ?? [])}"
                        );

                        Console.WriteLine(
                            $"Security token type: " +
                            $"{securityToken.GetType().Name}"
                        );

                        Console.WriteLine(
                            "===================================="
                        );

                        return tokenAudiences != null &&
                               tokenAudiences.Contains(audience);
                    }
            };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine();
                Console.WriteLine(
                    "========== JWT AUTHENTICATION FAILED =========="
                );

                Console.WriteLine(
                    context.Exception.ToString()
                );

                Console.WriteLine(
                    $"Configured audience: {audience}"
                );

                Console.WriteLine(
                    "==============================================="
                );

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine();
                Console.WriteLine(
                    "========== JWT TOKEN VALIDATED =========="
                );

                Console.WriteLine(
                    $"OID: {context.Principal?.FindFirst("oid")?.Value}"
                );

                Console.WriteLine(
                    $"TID: {context.Principal?.FindFirst("tid")?.Value}"
                );

                Console.WriteLine(
                    "=========================================="
                );

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();


// ===============================
// Build application
// ===============================

var app = builder.Build();


// ===============================
// Database Seeder
// ===============================

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<TenantGateDbContext>();

    await TenantGateDbSeeder.SeedAsync(dbContext);
}


// ===============================
// Middleware
// ===============================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Angular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();