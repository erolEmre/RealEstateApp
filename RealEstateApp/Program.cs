using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealEstate.Core.Interfaces;
using RealEstate.Core.Models;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Repository;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.OpenIdConnectEvents = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var sub = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = context.Principal.FindFirst("name")?.Value; // Burayı değiştirmeyi unutma name,
                                                                    // mail olarak döndüğü için şimdilik bunu verdim.

            var db = context.HttpContext.RequestServices
                .GetRequiredService<RealEstateContext>();

            var employee = db.Employees
                .FirstOrDefault(e => e.Auth0Sub == sub);

            if (employee == null)
            {
                employee = new Employee(sub, email);
                employee.FirstName = context.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                employee.LastName = context.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? "Unknown";
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
            }
        }
    };
    
});


builder.Services.AddDbContext<RealEstateContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IHouseRepository, HouseRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Veritabanını otomatik oluşturma ve tabloları basma kısmı
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RealEstateContext>(); // Kendi Context ismini yaz
        context.Database.Migrate(); // Bu satır eksik tabloları SQL'e basar
        Console.WriteLine("Veritabanı başarıyla güncellendi!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration sırasında hata oluştu: " + ex.Message);
    }
}
app.Run();