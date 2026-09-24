using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TenantGate.Api.Authorization;
using TenantGate.Api.Context;
using TenantGate.Application;
using TenantGate.Application.Context;
using TenantGate.Application.Services;
using TenantGate.Infrastructure;
using TenantGate.Infrastructure.Persistence;
using TenantGate.Api.Middleware;

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

                ValidAudience = audience
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
                Console.WriteLine("========== JWT TOKEN VALIDATED ==========");

                Console.WriteLine(
                    $"OID: {context.Principal?.FindFirst(
                        "http://schemas.microsoft.com/identity/claims/objectidentifier"
                    )?.Value}"
                );

                Console.WriteLine(
                    $"TenantId: {context.Principal?.FindFirst(
                        "http://schemas.microsoft.com/identity/claims/tenantid"
                    )?.Value}"
                );

                Console.WriteLine("==========================================");

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TenantAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(
            new TenantAccessRequirement());
    });
});


// ===============================
// Build application
// ===============================


builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<ITenantResolver, TenantResolver>();
builder.Services.AddScoped<ICurrentTenant, CurrentTenant>();

builder.Services.AddScoped<ITenantAuthorizationService, TenantAuthorizationService>();
builder.Services.AddScoped<IAuthorizationHandler, TenantAccessHandler>();


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
app.UseTenantInitialization();
app.UseAuthorization();

app.MapControllers();

app.Run();