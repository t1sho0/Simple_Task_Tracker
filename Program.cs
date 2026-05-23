using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        Console.WriteLine("--> Миграции успешно применились!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Ошибка наката миграций: {ex.Message}");
    }
}

app.Run();
