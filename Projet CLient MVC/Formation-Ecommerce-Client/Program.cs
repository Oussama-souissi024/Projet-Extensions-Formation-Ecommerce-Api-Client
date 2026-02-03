using Formation_Ecommerce_Client.Models.Configuration;
using Formation_Ecommerce_Client.Services.Interfaces;
using Formation_Ecommerce_Client.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Configuration des settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// 2. Configuration HttpClient
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// 3. Configuration de la Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. Injection des services API
builder.Services.AddScoped<IProductApiService, ProductApiService>();
builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<ICategoryApiService, CategoryApiService>();
builder.Services.AddScoped<ICartApiService, CartApiService>();
builder.Services.AddScoped<IOrderApiService, OrderApiService>();
builder.Services.AddScoped<ICouponApiService, CouponApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Utilisation de la Session
app.UseSession();

// Middleware de gestion des erreurs personnalisées
app.UseMiddleware<Formation_Ecommerce_Client.Helpers.ApiExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/// <summary>
/// Point d'entrée du Client MVC (UI) de l'extension "API + Client MVC".
/// </summary>
/// <remarks>
/// Différences pédagogiques vs le projet monolithique MVC :
/// - Cette application n'accède plus directement aux services Application/Infrastructure.
/// - Elle configure un <see cref="System.Net.Http.HttpClient"/> (nommé "ApiClient") pour consommer l'API REST (JSON/multipart).
/// - Le JWT est stocké côté client (ici via Session) et ajouté aux appels HTTP pour accéder aux endpoints protégés.
/// - Les contrôleurs MVC restent côté UI (Views Razor), mais délèguent la logique à des services HTTP (dossier Services).
/// </remarks>
public partial class Program { }
