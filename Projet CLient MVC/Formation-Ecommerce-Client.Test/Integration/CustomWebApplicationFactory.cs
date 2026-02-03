using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Formation_Ecommerce_Client.Services.Implementations;
using Formation_Ecommerce_Client.Test.Fakes;

namespace Formation_Ecommerce_Client.Test.Integration;

/// <summary>
/// Factory personnalisée pour les tests d'intégration du Client MVC.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - WebApplicationFactory permet de démarrer l'application en mémoire pour les tests.
/// - On remplace les services HTTP réels par des fakes pour contrôler les réponses API.
/// - Cela permet de tester les contrôleurs de bout en bout sans démarrer l'API réelle.
/// </remarks>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// FakeHttpMessageHandler partagé pour tous les tests.
    /// </summary>
    public FakeHttpMessageHandler HttpHandler { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Supprimer le IHttpClientFactory existant
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpClientFactory));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Ajouter notre FakeHttpClientFactory
            var fakeFactory = new FakeHttpClientFactory(HttpHandler);
            services.AddSingleton<IHttpClientFactory>(fakeFactory);

            // Configurer les faux services si nécessaire
            // services.AddScoped<IProductApiService, ...>();
        });
    }

    /// <summary>
    /// Crée un client HTTP configuré pour les tests.
    /// </summary>
    public HttpClient CreateTestClient()
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
}
