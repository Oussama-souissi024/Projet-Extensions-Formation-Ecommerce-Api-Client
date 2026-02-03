using Formation_Ecommerce_Client.Models.ApiResponses;
using System.Text.Json;

namespace Formation_Ecommerce_Client.Test.Common;

/// <summary>
/// Helper pour construire des réponses API simulées dans les tests.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - L'API retourne des réponses encapsulées dans ApiResponse&lt;T&gt;.
/// - Ce helper facilite la création de ces réponses dans les tests.
/// </remarks>
public static class ApiResponseBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Crée une réponse API réussie (Success = true).
    /// </summary>
    public static string Success<T>(T data, string? message = null)
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message ?? "Success",
            Data = data
        };
        return JsonSerializer.Serialize(response, JsonOptions);
    }

    /// <summary>
    /// Crée une réponse API en erreur (Success = false).
    /// </summary>
    public static string Error<T>(string message)
    {
        var response = new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default!
        };
        return JsonSerializer.Serialize(response, JsonOptions);
    }

    /// <summary>
    /// Crée une réponse API vide.
    /// </summary>
    public static string Empty()
    {
        return "{}";
    }
}
