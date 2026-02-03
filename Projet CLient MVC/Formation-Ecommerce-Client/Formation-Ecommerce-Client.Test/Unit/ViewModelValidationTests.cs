using Formation_Ecommerce_Client.Models.ViewModels.Products;
using Formation_Ecommerce_Client.Models.ViewModels.Categories;
using Formation_Ecommerce_Client.Test.Common;

namespace Formation_Ecommerce_Client.Test.Unit;

/// <summary>
/// Tests de validation des ViewModels avec DataAnnotations.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Vérifier que les attributs de validation ([Required], [Range], etc.) fonctionnent.
/// - Ces validations améliorent l'UX mais l'API reste la source de vérité.
/// </remarks>
public class ViewModelValidationTests
{
    #region CreateProductViewModel Tests

    [Fact]
    public void CreateProductViewModel_WithValidData_IsValid()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = "Produit Test",
            Price = 29.99m,
            Description = "Description valide",
            CategoryId = Guid.NewGuid()
        };

        // Act
        var isValid = ValidationHelper.IsValid(model);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void CreateProductViewModel_WithoutName_HasValidationError()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = null!,
            Price = 29.99m,
            CategoryId = Guid.NewGuid()
        };

        // Act
        var errors = ValidationHelper.GetErrorsForProperty(model, "Name");

        // Assert
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void CreateProductViewModel_WithShortName_HasValidationError()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = "A", // Moins de 2 caractères
            Price = 29.99m,
            CategoryId = Guid.NewGuid()
        };

        // Act
        var errors = ValidationHelper.GetErrorsForProperty(model, "Name");

        // Assert
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void CreateProductViewModel_WithZeroPrice_HasValidationError()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = "Produit Test",
            Price = 0m, // Prix invalide (< 0.01)
            CategoryId = Guid.NewGuid()
        };

        // Act
        var errors = ValidationHelper.GetErrorsForProperty(model, "Price");

        // Assert
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void CreateProductViewModel_WithNegativePrice_HasValidationError()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = "Produit Test",
            Price = -10m,
            CategoryId = Guid.NewGuid()
        };

        // Act
        var errors = ValidationHelper.GetErrorsForProperty(model, "Price");

        // Assert
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void CreateProductViewModel_WithoutCategory_HasValidationError()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = "Produit Test",
            Price = 29.99m,
            CategoryId = null
        };

        // Act
        var errors = ValidationHelper.GetErrorsForProperty(model, "CategoryId");

        // Assert
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void CreateProductViewModel_WithTooLongDescription_HasValidationError()
    {
        // Arrange
        var model = new CreateProductViewModel
        {
            Name = "Produit Test",
            Price = 29.99m,
            Description = new string('A', 1001), // Plus de 1000 caractères
            CategoryId = Guid.NewGuid()
        };

        // Act
        var errors = ValidationHelper.GetErrorsForProperty(model, "Description");

        // Assert
        Assert.NotEmpty(errors);
    }

    #endregion

    #region CategoryViewModel Tests

    [Fact]
    public void CreateCategoryViewModel_WithValidData_IsValid()
    {
        // Arrange
        var model = new CreateCategoryViewModel
        {
            Name = "Catégorie Test",
            Description = "Description valide"
        };

        // Act
        var isValid = ValidationHelper.IsValid(model);

        // Assert
        Assert.True(isValid);
    }

    #endregion
}
