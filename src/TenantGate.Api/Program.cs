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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();