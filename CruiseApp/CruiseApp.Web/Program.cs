using CruiseApp.Data;
using CruiseApp.Services.Core.Interfaces;
using CruiseApp.Services.Core.Services;
using CruiseApp.Common.Infrastructure;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CruiseApp.Web.Infrastructure;


// ===========================================
// LOAD .env (BEFORE builder)
// ===========================================
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// ===========================================
// Load JSON file: cabinDescriptions.json
// ===========================================
var path = Path.Combine(builder.Environment.ContentRootPath,
                        "Config",
                        "cabinDescriptions.json");

CabinDescriptionProvider.Load(path);

// ===========================================
// Add services to the container.
// ===========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found. Check .env file.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// ===========================================
builder.Services.AddScoped<ICruiseService, CruiseService>();
builder.Services.AddScoped<IShipService, ShipService>();
builder.Services.AddScoped<IPointService, PointService>();
builder.Services.AddScoped<ICruiseLikeService, CruiseLikeService>();
// ===========================================

var app = builder.Build();


// ===========================================
// AUTOMATIC DATABASE SEED
// ===========================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedRolesAsync(services);

    var db = services.GetRequiredService<ApplicationDbContext>();


    if (!await db.Ships.AnyAsync())
    {
        Console.WriteLine("Seeding database...");
        await DatabaseSeeder.SeedAsync(db);
    }
    else
    {
        Console.WriteLine("Database already contains data.");
    }
}



// ===========================================


// ===========================================
// Configure the HTTP request pipeline.
// ===========================================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
