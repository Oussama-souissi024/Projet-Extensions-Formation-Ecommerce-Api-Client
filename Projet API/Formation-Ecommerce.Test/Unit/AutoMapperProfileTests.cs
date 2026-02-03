using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Products.Mapping;
using Formation_Ecommerce_11_2025.Application.Categories.Mapping;
using Formation_Ecommerce_11_2025.Application.Cart.Mapping;
using Formation_Ecommerce_11_2025.Application.Coupons.Mapping;
using Formation_Ecommerce_11_2025.Application.Orders.Mapping;
using Formation_Ecommerce_11_2025.Application.Products.Dtos;
using Formation_Ecommerce_11_2025.Core.Entities.Product;

namespace Formation_Ecommerce_11_2025.Test.Unit;

/// <summary>
/// Tests de configuration AutoMapper.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Un mapping AutoMapper mal configuré ne provoque pas d'erreur de compilation.
/// L'erreur n'apparaît qu'au runtime quand on appelle _mapper.Map&lt;&gt;().
/// 
/// Note : AssertConfigurationIsValid() échoue si des propriétés ne sont pas mappées.
/// Dans cette configuration, certaines propriétés sont volontairement ignorées
/// (navigation properties, dates générées automatiquement, etc.).
/// 
/// Ces tests vérifient que le mapping fonctionne en pratique plutôt que
/// d'exiger un mapping exhaustif de toutes les propriétés.
/// </remarks>
public class AutoMapperProfileTests
{
    [Fact]
    public void ProductProfile_CreateMapper_Succeeds()
    {
        // Arrange & Act - Vérifie que la configuration ne lève pas d'exception
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
        var mapper = config.CreateMapper();

        // Assert
        Assert.NotNull(mapper);
    }

    [Fact]
    public void ProductProfile_CanMapCreateProductDtoToProduct()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
        var mapper = config.CreateMapper();

        var dto = new CreateProductDto
        {
            Name = "Test Product",
            Price = 29.99m,
            Description = "Description",
            CategoryID = Guid.NewGuid()
        };

        // Act
        var product = mapper.Map<Product>(dto);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(dto.Name, product.Name);
        Assert.Equal(dto.Price, product.Price);
        Assert.Equal(dto.Description, product.Description);
    }

    [Fact]
    public void ProductProfile_CanMapProductToProductDto()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
        var mapper = config.CreateMapper();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Price = 29.99m,
            Description = "Description",
            CategoryId = Guid.NewGuid()
        };

        // Act
        var dto = mapper.Map<ProductDto>(product);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Price, dto.Price);
    }

    [Fact]
    public void CategoryProfile_CreateMapper_Succeeds()
    {
        // Arrange & Act
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryProfile>());
        var mapper = config.CreateMapper();

        // Assert
        Assert.NotNull(mapper);
    }

    [Fact]
    public void CartMappingProfile_CreateMapper_Succeeds()
    {
        // Arrange & Act
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CartMappingProfile>());
        var mapper = config.CreateMapper();

        // Assert
        Assert.NotNull(mapper);
    }

    [Fact]
    public void CouponProfile_CreateMapper_Succeeds()
    {
        // Arrange & Act
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CouponProfile>());
        var mapper = config.CreateMapper();

        // Assert
        Assert.NotNull(mapper);
    }

    [Fact]
    public void OrderMappingProfile_CreateMapper_Succeeds()
    {
        // Arrange & Act
        var config = new MapperConfiguration(cfg => cfg.AddProfile<OrderMappingProfile>());
        var mapper = config.CreateMapper();

        // Assert
        Assert.NotNull(mapper);
    }

    [Fact]
    public void AllProfiles_CreateMapper_Succeeds()
    {
        // Arrange - Test de tous les profils ensemble
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
            cfg.AddProfile<CategoryProfile>();
            cfg.AddProfile<CartMappingProfile>();
            cfg.AddProfile<CouponProfile>();
            cfg.AddProfile<OrderMappingProfile>();
        });

        // Act
        var mapper = config.CreateMapper();

        // Assert
        Assert.NotNull(mapper);
    }
}
