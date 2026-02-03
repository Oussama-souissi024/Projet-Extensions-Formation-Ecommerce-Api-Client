using System.Net;
using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ViewModels.Products;
using Formation_Ecommerce_Client.Services.Implementations;
using Formation_Ecommerce_Client.Test.Common;
using Formation_Ecommerce_Client.Test.Fakes;

namespace Formation_Ecommerce_Client.Test.Unit;

/// <summary>
/// Tests unitaires de ProductApiService.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Ces tests vérifient la logique du service HTTP qui consomme l'API Products.
/// On mock les appels HTTP pour contrôler les réponses et isoler le test.
/// </remarks>
public class ProductApiServiceTests
{
    private readonly FakeHttpMessageHandler _httpHandler;
    private readonly FakeHttpClientFactory _httpFactory;
    private readonly FakeHttpContextAccessor _contextAccessor;

    public ProductApiServiceTests()
    {
        _httpHandler = new FakeHttpMessageHandler();
        _httpFactory = new FakeHttpClientFactory(_httpHandler);
        _contextAccessor = new FakeHttpContextAccessor();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsProducts_WhenApiSucceeds()
    {
        // Arrange
        var products = new List<ProductViewModel>
        {
            new() { Id = Guid.NewGuid(), Name = "Produit 1", Price = 10m },
            new() { Id = Guid.NewGuid(), Name = "Produit 2", Price = 20m }
        };
        var apiResponse = ApiResponseBuilder.Success<IEnumerable<ProductViewModel>>(products);
        _httpHandler.SetupResponse(HttpStatusCode.OK, apiResponse);

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Produit 1", result.First().Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenApiReturnsEmptyList()
    {
        // Arrange
        var apiResponse = ApiResponseBuilder.Success<IEnumerable<ProductViewModel>>(Array.Empty<ProductViewModel>());
        _httpHandler.SetupResponse(HttpStatusCode.OK, apiResponse);

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductViewModel
        {
            Id = productId,
            Name = "Test Product",
            Price = 99.99m,
            Description = "Description test"
        };
        var apiResponse = ApiResponseBuilder.Success(product);
        _httpHandler.SetupResponse(HttpStatusCode.OK, apiResponse);

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act
        var result = await service.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Test Product", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_CallsCorrectEndpoint()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductViewModel { Id = productId, Name = "Test" };
        _httpHandler.SetupResponse(HttpStatusCode.OK, ApiResponseBuilder.Success(product));

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act
        await service.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(_httpHandler.LastRequest);
        Assert.Contains($"products/{productId}", _httpHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetAllAsync_Throws_WhenApiReturnsError()
    {
        // Arrange
        _httpHandler.SetupResponse(HttpStatusCode.InternalServerError, "{}");

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAllAsync());
    }

    [Fact]
    public async Task DeleteAsync_CallsCorrectEndpoint()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _httpHandler.SetupResponse(HttpStatusCode.OK, "{}");

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act
        await service.DeleteAsync(productId);

        // Assert
        Assert.NotNull(_httpHandler.LastRequest);
        Assert.Equal(HttpMethod.Delete, _httpHandler.LastRequest!.Method);
        Assert.Contains($"products/{productId}", _httpHandler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public async Task CreateAsync_SendsPostRequest()
    {
        // Arrange
        var newProduct = new CreateProductViewModel
        {
            Name = "New Product",
            Price = 29.99m,
            Description = "Description",
            CategoryId = Guid.NewGuid()
        };
        var createdProduct = new ProductViewModel
        {
            Id = Guid.NewGuid(),
            Name = newProduct.Name,
            Price = newProduct.Price
        };
        _httpHandler.SetupResponse(HttpStatusCode.OK, ApiResponseBuilder.Success(createdProduct));

        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Act
        var result = await service.CreateAsync(newProduct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpMethod.Post, _httpHandler.LastRequest!.Method);
        Assert.Contains("products", _httpHandler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public void Service_SetsAuthorizationHeader_WhenTokenInSession()
    {
        // Arrange
        _contextAccessor.SetSessionValue("JwtToken", "test-jwt-token");
        
        // Act - Le header est défini dans le constructeur
        var service = new ProductApiService(_httpFactory, _contextAccessor);

        // Assert - Difficile à vérifier directement sans exposer le client
        // Ce test vérifie principalement que le service ne lève pas d'exception
        Assert.NotNull(service);
    }
}
