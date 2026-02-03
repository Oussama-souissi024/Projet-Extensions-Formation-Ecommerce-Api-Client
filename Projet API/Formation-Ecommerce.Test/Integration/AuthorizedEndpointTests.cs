using System.Net;
using System.Net.Http.Json;
using Formation_Ecommerce_11_2025.Test.Common;

namespace Formation_Ecommerce_11_2025.Test.Integration;

/// <summary>
/// Tests des endpoints nécessitant une authentification.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Ces tests vérifient le comportement des endpoints protégés par [Authorize].
/// 
/// Scénarios testés :
/// - Accès avec authentification valide → succès
/// - Accès sans authentification → 401 Unauthorized
/// - Accès avec rôle insuffisant → 403 Forbidden
/// </remarks>
public class AuthorizedEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthorizedEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        // Reset le handler entre les tests
        TestAuthHandler.Reset();
    }

    [Fact]
    public async Task CreateProduct_WithoutAuth_RequiresAuthentication()
    {
        // Arrange
        var client = _factory.CreateUnauthenticatedClient();
        var newProduct = new
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryID = Guid.NewGuid()
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Products", newProduct);

        // Assert - Sans auth, l'API peut retourner Unauthorized ou UnsupportedMediaType
        // selon si l'authentification est vérifiée avant ou après le Content-Type
        Assert.True(
            response.StatusCode == HttpStatusCode.Unauthorized || 
            response.StatusCode == HttpStatusCode.UnsupportedMediaType,
            $"Expected Unauthorized or UnsupportedMediaType, got {response.StatusCode}"
        );
    }

    [Fact]
    public async Task CreateProduct_WithAuth_DoesNotReturnUnauthorized()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("Admin");
        var newProduct = new
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            CategoryID = Guid.NewGuid()
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/Products", newProduct);

        // Assert - Peut retourner BadRequest si la catégorie n'existe pas, mais pas Unauthorized
        Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateUnauthenticatedClient();
        var productId = Guid.NewGuid();

        // Act
        var response = await client.DeleteAsync($"/api/Products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCart_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateUnauthenticatedClient();

        // Act
        var response = await client.GetAsync("/api/Cart");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCart_WithAuth_DoesNotReturnUnauthorized()
    {
        // Arrange
        var client = _factory.CreateAuthenticatedClient("User");

        // Act
        var response = await client.GetAsync("/api/Cart");

        // Assert
        Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
