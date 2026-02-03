using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Products.Dtos;
using Formation_Ecommerce_11_2025.Application.Products.Mapping;
using Formation_Ecommerce_11_2025.Application.Products.Services;
using Formation_Ecommerce_11_2025.Core.Entities.Category;
using Formation_Ecommerce_11_2025.Core.Entities.Product;
using Formation_Ecommerce_11_2025.Test.Fakes;
using Microsoft.AspNetCore.Http;

namespace Formation_Ecommerce_11_2025.Test.Unit;

/// <summary>
/// Tests unitaires de ProductServices (CRUD + gestion d'image).
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Ces tests vérifient la logique métier du service de produits en isolation,
/// sans démarrer le serveur web ni accéder à une vraie base de données.
/// 
/// Stratégie :
/// - Utilisation de Fakes pour les dépendances (repositories, file helper)
/// - Tests des différents scénarios (avec/sans image, produit introuvable, etc.)
/// </remarks>
public class ProductServicesTests
{
    /// <summary>
    /// Crée un mapper AutoMapper configuré avec le profil Product.
    /// </summary>
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
        return config.CreateMapper();
    }

    [Fact]
    public async Task AddAsync_WithoutImage_DoesNotUploadFile()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        var dto = new CreateProductDto
        {
            Name = "Produit Test",
            Price = 10m,
            Description = "Description test",
            CategoryID = Guid.NewGuid(),
            ImageFile = null
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, fileHelper.UploadCallCount);
        Assert.Null(result!.ImageUrl);
    }

    [Fact]
    public async Task AddAsync_WithImage_UploadsFileAndSetsImageUrl()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper { UploadReturnValue = "new-product.png" };
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        // Créer un faux fichier image
        var imageContent = new byte[] { 1, 2, 3, 4, 5 };
        var file = new FormFile(
            new MemoryStream(imageContent), 
            0, 
            imageContent.Length, 
            "ImageFile", 
            "product-image.png"
        );

        var dto = new CreateProductDto
        {
            Name = "Produit avec image",
            Price = 29.99m,
            Description = "Description avec image",
            CategoryID = Guid.NewGuid(),
            ImageFile = file
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, fileHelper.UploadCallCount);
        Assert.Equal("new-product.png", result!.ImageUrl);
    }

    [Fact]
    public async Task ReadByIdAsync_WhenNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.ReadByIdAsync(Guid.NewGuid())
        );
    }

    [Fact]
    public async Task ReadByIdAsync_WhenFound_ReturnsProduct()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Catégorie Test",
            Description = "Description",
            Products = new List<Product>()
        };

        var product = await productRepo.AddAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Produit Existant",
            Description = "Description",
            Price = 15m,
            CategoryId = category.Id,
            Category = category,
            CartDetails = new List<Formation_Ecommerce_11_2025.Core.Entities.Cart.CartDetails>()
        });

        // Act
        var result = await service.ReadByIdAsync(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Price, result.Price);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductHasImage_DeletesFile()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Catégorie",
            Description = "Description",
            Products = new List<Product>()
        };

        var product = await productRepo.AddAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Produit avec image",
            Description = "Description",
            Price = 10m,
            ImageUrl = "old-image.png",
            CategoryId = category.Id,
            Category = category,
            CartDetails = new List<Formation_Ecommerce_11_2025.Core.Entities.Cart.CartDetails>()
        });

        // Act
        await service.DeleteAsync(product.Id);

        // Assert
        Assert.Equal(1, fileHelper.DeleteCallCount);
        Assert.Equal("old-image.png", fileHelper.LastDeletedPath);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductHasNoImage_DoesNotDeleteFile()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Catégorie",
            Description = "Description",
            Products = new List<Product>()
        };

        var product = await productRepo.AddAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Produit sans image",
            Description = "Description",
            Price = 10m,
            ImageUrl = null,
            CategoryId = category.Id,
            Category = category,
            CartDetails = new List<Formation_Ecommerce_11_2025.Core.Entities.Cart.CartDetails>()
        });

        // Act
        await service.DeleteAsync(product.Id);

        // Assert
        Assert.Equal(0, fileHelper.DeleteCallCount);
    }

    [Fact]
    public async Task ReadProductsByCategoryName_WhenCategoryNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService { CategoryIdByName = null };
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.ReadProductsByCategoryName("Catégorie Inexistante")
        );
    }

    [Fact]
    public async Task UpdateAsync_WithNewImage_UploadsAndDeletesOldImage()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper { UploadReturnValue = "new-image.png" };
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Catégorie",
            Description = "Description",
            Products = new List<Product>()
        };

        var existingProduct = await productRepo.AddAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = "Ancien nom",
            Description = "Ancienne description",
            Price = 10m,
            ImageUrl = "old-image.png",
            CategoryId = category.Id,
            Category = category,
            CartDetails = new List<Formation_Ecommerce_11_2025.Core.Entities.Cart.CartDetails>()
        });

        var newImage = new FormFile(
            new MemoryStream(new byte[] { 1 }), 
            0, 1, 
            "ImageFile", 
            "new-image.png"
        );

        var updateDto = new UpdateProductDto
        {
            Id = existingProduct.Id,
            Name = "Nouveau nom",
            Price = 20m,
            Description = "Nouvelle description",
            CategoryId = category.Id,
            ImageFile = newImage
        };

        // Act
        await service.UpdateAsync(updateDto);

        // Assert
        Assert.Equal(1, fileHelper.UploadCallCount);
        Assert.Equal(1, fileHelper.DeleteCallCount);
        Assert.Equal("new-image.png", productRepo.LastUpdatedProduct?.ImageUrl);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductNotFound_ThrowsException()
    {
        // Arrange
        var productRepo = new FakeProductRepository();
        var fileHelper = new FakeFileHelper();
        var categoryService = new FakeCategoryService();
        var service = new ProductServices(productRepo, CreateMapper(), categoryService, fileHelper);

        var updateDto = new UpdateProductDto
        {
            Id = Guid.NewGuid(),
            Name = "Nom",
            Price = 10m,
            CategoryId = Guid.NewGuid()
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(updateDto));
        Assert.Contains("Erreur lors de la mise à jour du produit", ex.Message);
    }
}
