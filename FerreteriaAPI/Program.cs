using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// MySQL + EF Core
// =========================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Si existen variables de entorno (Railway), las usamos
var host = Environment.GetEnvironmentVariable("MYSQLHOST");
var port = Environment.GetEnvironmentVariable("MYSQLPORT");
var database = Environment.GetEnvironmentVariable("MYSQLDATABASE");
var username = Environment.GetEnvironmentVariable("MYSQLUSER");
var password = Environment.GetEnvironmentVariable("MYSQLPASSWORD");

if (!string.IsNullOrEmpty(host))
{
    connectionString =
        $"server={host};port={port};database={database};user={username};password={password}";
}

builder.Services.AddDbContext<FerreteriaDbContext>(options =>
{
    var connectionString =
        builder.Configuration["ConnectionStrings:DefaultConnection"];

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

// =========================
// CORS
// =========================

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("ReactPolicy");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();