using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FerreteriaWeb.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("FerreteriaApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5248/");
});

// Identity con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ferreteriaUsers.db"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/Login";
});

// TempData con sesión
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Crear usuarios iniciales
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Crear roles
    if (!await roleManager.RoleExistsAsync("Administrador"))
        await roleManager.CreateAsync(new IdentityRole("Administrador"));
    if (!await roleManager.RoleExistsAsync("Desarrollador"))
        await roleManager.CreateAsync(new IdentityRole("Desarrollador"));

    // Usuario dueño
    if (await userManager.FindByEmailAsync("dueno@ferreteria.com") == null)
    {
        var dueno = new IdentityUser { UserName = "dueno@ferreteria.com", Email = "dueno@ferreteria.com" };
        await userManager.CreateAsync(dueno, "dueno123");
        await userManager.AddToRoleAsync(dueno, "Administrador");
    }

    // Usuario desarrollador (tú)
    if (await userManager.FindByEmailAsync("fernando@ferreteria.com") == null)
    {
        var dev = new IdentityUser { UserName = "fernando@ferreteria.com", Email = "fernando@ferreteria.com" };
        await userManager.CreateAsync(dev, "dev123");
        await userManager.AddToRoleAsync(dev, "Desarrollador");
    }
}

app.Run();