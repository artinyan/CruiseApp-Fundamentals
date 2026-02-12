using CruiseApp.Data;
using CruiseApp.Services.Interfaces;
using CruiseApp.Services.Services;
using DotNetEnv; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CruiseApp.Web.Infrastructure;

// ===========================================
// LOAD .env (BEFORE builder)
// ===========================================
Env.Load();
//Console.WriteLine(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
//                  ?? "❌ Connection string is NULL");

var builder = WebApplication.CreateBuilder(args);

//
builder.Configuration.AddEnvironmentVariables();

// ===========================================
// Add services to the container.
// ===========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found. Check .env file.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//        options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
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
// ===========================================

var app = builder.Build();

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




using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
}

app.Run();
