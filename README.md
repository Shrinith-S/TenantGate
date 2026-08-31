TenantGate Development Documentation

This document records the TenantGate architecture, progress, authentication work, tenant service work, troubleshooting, and next steps completed so far.

Project

TenantGate is a learning/architecture project using Angular, ASP.NET Core, Microsoft Entra ID/MSAL, JWT bearer authentication, EF Core, and SQL Server, with a focus on multi-tenant authentication, tenant binding, and tenant isolation.

Architecture

Angular Frontend
   -> MSAL access token
TenantGate.Api
   -> Authentication / Authorization
TenantGate.Application
   -> TenantService / Repository abstraction
TenantGate.Infrastructure
   -> EF Core / SQL Server
TenantGateDb

Solution Structure

src/
  TenantGate.Api/
    Controllers/
    Program.cs
    appsettings.json
  TenantGate.Application/
    Services/
      ITenantService.cs
      TenantService.cs
      TenantDto.cs
    ITenantRepository.cs
  TenantGate.Domain/
    Entities/
  TenantGate.Infrastructure/
    Persistence/
      TenantGateDbContext.cs
      TenantGateDbSeeder.cs
      Migrations/
    TenantRepository.cs
frontend/
  tenant-gate-web/

Completed

Angular frontend and ASP.NET Core backend created and connected.

SQL Server database and EF Core DbContext configured.

EF Core migrations and tenant seeding established.

GET /api/tenants implemented and verified.

Angular tenant list implemented.

MSAL configured in Angular.

MSAL interceptor configured so API calls send Authorization: Bearer <token>.

CORS configured for http://localhost:4200.

ASP.NET Core JWT bearer authentication configured.

JWT audience mismatch was diagnosed and corrected.

Tenant service abstraction introduced.

Repository abstraction and Infrastructure implementation introduced.

Frontend changes committed and pushed to main.

Tenant API

The API exposes:

GET /api/tenants

The response contains tenant Id, EntraTenantId, Name, and IsActive.

Application Layer

Current service contract:

public interface ITenantService
{
    Task<IReadOnlyList<TenantDto>> GetTenantsAsync();
}

The current service uses EF Core directly. The intended next refactoring is:

Controller -> ITenantService -> ITenantRepository -> TenantRepository -> TenantGateDbContext

so Application does not directly depend on Infrastructure.

Repository

Current contract:

public interface ITenantRepository
{
    Task<IReadOnlyList<Tenant>> GetActiveTenantsAsync();
}

TenantRepository.GetActiveTenantsAsync() still needs its database implementation.

Authentication

Angular authenticates with Microsoft Entra ID using MSAL and obtains an access token for the TenantGate API.

The browser was verified to send:

Authorization: Bearer <access-token>

The backend uses:

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority =
            $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/v2.0";

        options.Audience =
            builder.Configuration["AzureAd:Audience"];
    });

Middleware order:

UseAuthentication()
UseAuthorization()
MapControllers()

JWT Troubleshooting

Missing authentication scheme

The error:

No authenticationScheme was specified, and there was no DefaultChallengeScheme found.

was caused by authentication not being registered.

Read-only service collection

Authentication was initially registered after builder.Build(), causing:

The service collection cannot be modified because it is read-only.

All builder.Services.Add... registrations must happen before builder.Build().

Audience validation

The API initially returned 401 with IDX10214 audience validation failure.
Debugging showed the configured audience and token audience differed by a character. Once aligned, the request returned 200 OK with the bearer token present.

CORS

Configured for the Angular development origin:

http://localhost:4200

The response was verified to include the expected Access-Control-Allow-Origin header.

Tenant Binding — Next Major Feature

The next security feature is tenant binding. The authenticated JWT contains the Entra tenant ID claim tid.

Intended flow:

Authenticated JWT
   -> tid
   -> Tenant lookup by EntraTenantId
   -> TenantContext
   -> tenant-isolated application operations

The API should derive the tenant from trusted authentication information rather than trusting an arbitrary tenant ID supplied by the browser.

Tenant Isolation Goal

A request such as tenantId=another-tenant must not allow a user to access another tenant. Tenant context should be established from the authenticated identity and applied to data access.

Token Introspection

We discussed shortening the Entra access token through introspection. Introspection is not simply a way to shorten a Microsoft Entra JWT. MSAL normally receives JWT access tokens, and the API can validate them locally using Entra signing keys. The project will continue with JWT bearer validation rather than introducing introspection solely to reduce token size.

Git

Repository: Shrinith-S/TenantGate
Branch: main
Latest frontend commit created during the session:

7464062 - Persist selected tenant in Angular and Bearer Token add to header

The local working tree was verified clean after push.

Next Development Sequence

Implement TenantRepository.GetActiveTenantsAsync().

Refactor TenantService to use ITenantRepository.

Update TenantsController to use ITenantService.

Extract tid from the authenticated JWT.

Find the matching tenant.

Create/register a per-request TenantContext.

Enforce tenant filtering/isolation.

Add authorization policies/roles.

Add automated tests.

Continue toward Azure/AKS deployment architecture.

Security Rules

Never commit access tokens, refresh tokens, client secrets, database passwords, private keys, or production credentials. Use placeholders and appropriate secret management for sensitive configuration.
