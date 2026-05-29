using Auth0.AspNetCore.Authentication;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealEstate.Application.AutoMapper;
using RealEstate.Application.Interfaces;
using RealEstate.Core.Interfaces.Houses.HouseRepository;
using RealEstate.Core.Models;
using RealEstate.Infrastructure.Context;
using RealEstate.Infrastructure.Repository;
using RealEstate.Infrastructure.Services;
using RealEstateApp.WebUI;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

// Program.cs

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllersWithViews();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    if (string.IsNullOrEmpty(options.Domain) || string.IsNullOrEmpty(options.ClientId))
{
    
    throw new Exception("HATA: Auth0 konfigürasyon değerleri okunamadı! .env dosyasını veya Docker environment değişkenlerini kontrol edin.");
}
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];

    options.OpenIdConnectEvents = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            var claims = context.Principal.Claims;
            var sub = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Auth0 genellikle emaili "email" veya ClaimTypes.Email olarak gönderir
            var email = claims.FirstOrDefault(c => c.Type == "email")?.Value
                        ?? claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var db = context.HttpContext.RequestServices.GetRequiredService<RealEstateContext>();
            var employee = await db.Employees.FirstOrDefaultAsync(e => e.Auth0Sub == sub);

            if (employee == null)
            {
                // Yeni çalışan oluştur
                employee = new Employee(sub, email ?? "no-email@domain.com");

                employee.FirstName = claims.FirstOrDefault(c => c.Type == "given_name")?.Value
                                    ?? claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value
                                    ?? "Unknown";

                employee.LastName = claims.FirstOrDefault(c => c.Type == "family_name")?.Value
                                   ?? claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value
                                   ?? "Unknown";

                db.Employees.Add(employee);
                await db.SaveChangesAsync();
            }
        }
    };
    
});

builder.Services.AddScoped<IClaimsTransformation, ClaimsTransformer>();

builder.Services.AddDbContext<RealEstateContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<IHouseFilterService, HouseFilterService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

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
    pattern: "{controller=Houses}/{action=Index}/{id?}");

// Veritabanını otomatik oluşturma ve tabloları basma kısmı
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RealEstateContext>();
        context.Database.EnsureCreated();
        context.Database.Migrate(); // Bu satır eksik tabloları SQL'e basar
        Console.WriteLine("Veritabanı başarıyla güncellendi!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration sırasında hata oluştu: " + ex.Message);
    }
}

app.Run();