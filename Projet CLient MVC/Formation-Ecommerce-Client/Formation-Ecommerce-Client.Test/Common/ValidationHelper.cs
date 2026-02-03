using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Test.Common;

/// <summary>
/// Helper de tests pour valider un objet via les DataAnnotations.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// Dans une application ASP.NET Core, la validation des DataAnnotations est faite automatiquement
/// via ModelBinding / ModelState. En tests unitaires, on reproduit cette validation manuellement.
/// </remarks>
public static class ValidationHelper
{
    /// <summary>
    /// Valide un objet et retourne la liste des erreurs de validation.
    /// </summary>
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
    public static bool IsValid(object model)
    {
        return Validate(model).Count == 0;
    }

    /// <summary>
    /// Obtient les messages d'erreur pour une propriété spécifique.
    /// </summary>
    public static IEnumerable<string> GetErrorsForProperty(object model, string propertyName)
    {
        return Validate(model)
            .Where(r => r.MemberNames.Contains(propertyName))
            .Select(r => r.ErrorMessage ?? "Validation error");
    }
}
