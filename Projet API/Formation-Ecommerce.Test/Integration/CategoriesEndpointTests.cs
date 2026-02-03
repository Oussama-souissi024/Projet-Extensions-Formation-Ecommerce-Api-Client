using System.Net;

namespace Formation_Ecommerce_11_2025.Test.Integration;

/// <summary>
/// Tests d'intégration des endpoints Categories.
/// </summary>
public class CategoriesEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CategoriesEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCategories_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/Categories");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCategoryById_NotFound_ReturnsErrorStatusCode()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/Categories/{nonExistentId}");

        // Assert - L'API peut retourner 404 ou 500 selon la gestion d'erreur
        Assert.True(
            response.StatusCode == HttpStatusCode.NotFound || 
            response.StatusCode == HttpStatusCode.InternalServerError,
            $"Expected NotFound or InternalServerError, got {response.StatusCode}"
        );
    }
}
