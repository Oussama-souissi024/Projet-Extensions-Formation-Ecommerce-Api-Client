using System.Net;
using Formation_Ecommerce_Client.Models.ViewModels.Products;
using Formation_Ecommerce_Client.Test.Common;

namespace Formation_Ecommerce_Client.Test.Integration;

/// <summary>
/// Tests d'intégration du ProductController.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Ces tests vérifient le comportement complet des actions du contrôleur Product,
/// du routing jusqu'au rendu de la vue, en mockant les appels API.
/// </remarks>
public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProductControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateTestClient();
    }

    [Fact]
    public async Task ProductIndex_ReturnsView_WhenApiSucceeds()
    {
        // Arrange
        var products = new List<ProductViewModel>
        {
            new() { Id = Guid.NewGuid(), Name = "Produit 1", Price = 10m },
            new() { Id = Guid.NewGuid(), Name = "Produit 2", Price = 20m }
        };
        _factory.HttpHandler.SetupResponse(
            HttpStatusCode.OK, 
            ApiResponseBuilder.Success<IEnumerable<ProductViewModel>>(products)
        );

        // Act
        var response = await _client.GetAsync("/Product/ProductIndex");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ProductIndex_ReturnsView_WhenApiReturnsEmpty()
    {
        // Arrange
        _factory.HttpHandler.SetupResponse(
            HttpStatusCode.OK,
            ApiResponseBuilder.Success<IEnumerable<ProductViewModel>>(Array.Empty<ProductViewModel>())
        );

        // Act
        var response = await _client.GetAsync("/Product/ProductIndex");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_GET_ReturnsView()
    {
        // Arrange - Mock la liste des catégories
        var categories = new List<object> { new { Id = Guid.NewGuid(), Name = "Cat 1" } };
        _factory.HttpHandler.SetupResponse(HttpStatusCode.OK, ApiResponseBuilder.Success(categories));

        // Act
        var response = await _client.GetAsync("/Product/CreateProduct");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Details_ReturnsView_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductViewModel
        {
            Id = productId,
            Name = "Test Product",
            Price = 99.99m
        };
        _factory.HttpHandler.SetupResponse(HttpStatusCode.OK, ApiResponseBuilder.Success(product));

        // Act
        var response = await _client.GetAsync($"/Product/Details/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_GET_ReturnsView_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductViewModel
        {
            Id = productId,
            Name = "Product to Delete",
            Price = 49.99m
        };
        _factory.HttpHandler.SetupResponse(HttpStatusCode.OK, ApiResponseBuilder.Success(product));

        // Act
        var response = await _client.GetAsync($"/Product/DeleteProduct/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
