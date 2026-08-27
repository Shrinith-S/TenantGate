using Microsoft.EntityFrameworkCore;
using TenantGate.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<TenantGateDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TenantGateDb"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<TenantGateDbContext>();

    await TenantGateDbSeeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();