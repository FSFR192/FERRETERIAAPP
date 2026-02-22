using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MySQL + EF Core
builder.Services.AddDbContext<FerreteriaDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
});


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

// Pipeline

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
