using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_11_2025.Test.Common;

/// <summary>
/// Helper de tests pour valider un objet via les DataAnnotations.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Dans une application ASP.NET Core, la validation des DataAnnotations est faite automatiquement
/// via ModelBinding / ModelState. En tests unitaires, on reproduit cette validation manuellement.
/// 
/// Exemple d'utilisation :
/// <code>
/// var dto = new CreateProductDto { Name = "" };
/// var results = ValidationHelper.Validate(dto);
/// Assert.NotEmpty(results); // Name est requis
/// </code>
/// </remarks>
public static class ValidationHelper
{
    /// <summary>
    /// Valide un objet et retourne la liste des erreurs de validation.
    /// </summary>
    /// <param name="model">L'objet à valider</param>
    /// <returns>Liste des erreurs de validation (vide si tout est valide)</returns>
    public static IList<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }

    /// <summary>
    /// Vérifie si un objet est valide selon ses DataAnnotations.
    /// </summary>
    /// <param name="model">L'objet à valider</param>
    /// <returns>True si l'objet est valide, False sinon</returns>
    public static bool IsValid(object model)
    {
        return Validate(model).Count == 0;
    }

    /// <summary>
    /// Obtient les messages d'erreur pour une propriété spécifique.
    /// </summary>
    /// <param name="model">L'objet à valider</param>
    /// <param name="propertyName">Nom de la propriété</param>
    /// <returns>Liste des messages d'erreur pour cette propriété</returns>
    public static IEnumerable<string> GetErrorsForProperty(object model, string propertyName)
    {
        return Validate(model)
            .Where(r => r.MemberNames.Contains(propertyName))
            .Select(r => r.ErrorMessage ?? "Validation error");
    }
}
