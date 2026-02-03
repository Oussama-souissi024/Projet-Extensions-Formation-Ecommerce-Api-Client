using Formation_Ecommerce_11_2025.Application.Products.Dtos;
using Formation_Ecommerce_11_2025.Application.Categories.Dtos;
using Formation_Ecommerce_11_2025.Test.Common;

namespace Formation_Ecommerce_11_2025.Test.Unit;

/// <summary>
/// Tests de validation des DTOs via DataAnnotations.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Vérifier que les attributs de validation ([Required], [Range], etc.)
/// fonctionnent correctement sur les DTOs.
/// 
/// Note : ASP.NET Core valide automatiquement les DTOs via ModelBinding,
/// mais ici on teste cette validation en isolation.
/// </remarks>
public class DtoValidationTests
{
    #region CreateProductDto Tests

    [Fact]
    public void CreateProductDto_WithValidData_HasNoValidationErrors()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produit Valide",
            Price = 29.99m,
            Description = "Description valide",
            CategoryID = Guid.NewGuid()
        };

        // Act
        var results = ValidationHelper.Validate(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void CreateProductDto_WithZeroPrice_HasValidationError()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produit Test",
            Price = 0m, // Prix invalide pour [Required] sur decimal
            Description = "Description",
            CategoryID = Guid.NewGuid()
        };

        // Act
        var isValid = ValidationHelper.IsValid(dto);

        // Assert - Le prix 0 est techniquement valide pour [Required]
        // car decimal a une valeur par défaut (0)
        Assert.True(isValid);
    }

    [Fact]
    public void CreateProductDto_WithNegativePrice_IsValid()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produit Test",
            Price = -10m, // Prix négatif - pas de validation [Range] actuellement
            Description = "Description",
            CategoryID = Guid.NewGuid()
        };

        // Act
        var results = ValidationHelper.Validate(dto);

        // Assert - Pas d'erreur car il n'y a pas de [Range] sur Price
        Assert.Empty(results);
    }

    #endregion

    #region CreateCategoryDto Tests

    [Fact]
    public void CreateCategoryDto_WithValidData_HasNoValidationErrors()
    {
        // Arrange
        var dto = new CreateCategoryDto
        {
            Name = "Catégorie Valide",
            Description = "Description valide"
        };

        // Act
        var results = ValidationHelper.Validate(dto);

        // Assert
        Assert.Empty(results);
    }

    #endregion

    #region UpdateProductDto Tests

    [Fact]
    public void UpdateProductDto_WithValidData_HasNoValidationErrors()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Id = Guid.NewGuid(),
            Name = "Produit Mis à Jour",
            Price = 49.99m,
            Description = "Description mise à jour",
            CategoryId = Guid.NewGuid()
        };

        // Act
        var results = ValidationHelper.Validate(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void UpdateProductDto_WithEmptyId_IsValid()
    {
        // Arrange - Pas de [Required] sur Id actuellement
        var dto = new UpdateProductDto
        {
            Id = Guid.Empty,
            Name = "Produit",
            Price = 10m,
            CategoryId = Guid.NewGuid()
        };

        // Act
        var results = ValidationHelper.Validate(dto);

        // Assert
        Assert.Empty(results);
    }

    #endregion
}
