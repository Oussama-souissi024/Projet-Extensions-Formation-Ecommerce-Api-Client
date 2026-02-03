using Formation_Ecommerce_11_2025.Infrastructure.Persistence;
using Formation_Ecommerce_11_2025.Test.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Formation_Ecommerce_11_2025.Test.Integration;

/// <summary>
/// Factory de tests d'intégration basée sur <see cref="WebApplicationFactory{TEntryPoint}" />.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Cette factory permet de démarrer l'API ASP.NET Core "en mémoire" pour des tests HTTP.
/// 
/// Points clés :
/// - Remplacement de la DB par EF Core InMemory (pas de SQL Server requis)
/// - Remplacement de l'authentification JWT par un handler de test
/// - Nom de base unique pour éviter les interférences entre tests parallèles
/// 
/// Utilisation :
/// <code>
/// public class MyTests : IClassFixture&lt;CustomWebApplicationFactory&gt;
/// {
///     private readonly HttpClient _client;
///     
///     public MyTests(CustomWebApplicationFactory factory)
///     {
///         _client = factory.CreateClient();
///     }
/// }
/// </code>
/// </remarks>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            // 1. Supprimer la configuration de la base de données SQL Server
            var dbContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            // Supprimer aussi le DbContext lui-même s'il est enregistré
            var dbContextServiceDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(ApplicationDbContext));

            if (dbContextServiceDescriptor != null)
                services.Remove(dbContextServiceDescriptor);

            // 2. Ajouter la base de données InMemory avec un nom unique
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase($"FormationEcommerceApiTests_{Guid.NewGuid()}");
            });

            // 3. Remplacer l'authentification JWT par un handler de test
            // Supprimer les schémas d'authentification existants
            var authSchemeDescriptors = services
                .Where(d => d.ServiceType.FullName?.Contains("Authentication") == true)
                .ToList();

            // Ajouter notre schéma de test
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme, options => { });

            // 4. S'assurer que la DB est créée et prête
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
        });
    }

    /// <summary>
    /// Crée un client HTTP authentifié pour les tests.
    /// </summary>
    public HttpClient CreateAuthenticatedClient(string role = "User")
    {
        TestAuthHandler.TestUserRole = role;
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        return client;
    }

    /// <summary>
    /// Crée un client HTTP non authentifié pour les tests.
    /// </summary>
    public HttpClient CreateUnauthenticatedClient()
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
}
