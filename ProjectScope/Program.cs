using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectScope.Data;
using ProjectScope.Data.Repo;
using ProjectScope.Entity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EntityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "Authenticated";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
            options.SlidingExpiration = true;
        });
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".ProjectScope.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Adjust timeout as needed
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStudent, SqlRegistrationTable>();
builder.Services.AddScoped<IContact, SqlContactTable>();
builder.Services.AddScoped<ICourse , SqlCourseTable>();


builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/WebHome/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WebHome}/{action=Index}/{id?}");

app.Run();
