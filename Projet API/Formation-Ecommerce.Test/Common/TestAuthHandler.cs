using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Formation_Ecommerce_11_2025.Test.Common;

/// <summary>
/// Handler d'authentification de test qui bypasse JWT pour les tests d'intégration.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Les endpoints protégés par [Authorize] nécessitent un token JWT valide.
/// - Ce handler remplace l'authentification JWT par une authentification "fake".
/// - Permet de simuler différents utilisateurs et rôles sans générer de vrais tokens.
/// 
/// Utilisation dans CustomWebApplicationFactory :
/// <code>
/// services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
///     .AddScheme&lt;AuthenticationSchemeOptions, TestAuthHandler&gt;(
///         TestAuthHandler.AuthenticationScheme, options => { });
/// </code>
/// 
/// Utilisation dans les tests :
/// <code>
/// TestAuthHandler.TestUserRole = "Admin"; // Simuler un admin
/// _client.DefaultRequestHeaders.Authorization = 
///     new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
/// </code>
/// </remarks>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    /// Nom du schéma d'authentification de test.
    /// </summary>
    public const string AuthenticationScheme = "TestScheme";

    /// <summary>
    /// ID de l'utilisateur de test par défaut.
    /// </summary>
    public const string DefaultUserId = "test-user-id";

    /// <summary>
    /// Email de l'utilisateur de test par défaut.
    /// </summary>
    public const string DefaultUserEmail = "test@example.com";

    /// <summary>
    /// Rôle à assigner à l'utilisateur de test.
    /// Modifiable pour tester différents rôles (Admin, User, etc.).
    /// </summary>
    public static string? TestUserRole { get; set; } = "User";

    /// <summary>
    /// ID personnalisé pour l'utilisateur de test.
    /// Si null, utilise DefaultUserId.
    /// </summary>
    public static string? CustomUserId { get; set; }

    /// <summary>
    /// Email personnalisé pour l'utilisateur de test.
    /// Si null, utilise DefaultUserEmail.
    /// </summary>
    public static string? CustomUserEmail { get; set; }

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Vérifier si le header Authorization est présent avec le bon schéma
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith(AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // Créer les claims de l'utilisateur de test
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, CustomUserId ?? DefaultUserId),
            new(ClaimTypes.Email, CustomUserEmail ?? DefaultUserEmail),
            new(ClaimTypes.Name, "Test User"),
        };

        // Ajouter le rôle si spécifié
        if (!string.IsNullOrEmpty(TestUserRole))
        {
            claims.Add(new Claim(ClaimTypes.Role, TestUserRole));
        }

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    /// <summary>
    /// Réinitialise les paramètres de test à leurs valeurs par défaut.
    /// À appeler entre les tests pour éviter les interférences.
    /// </summary>
    public static void Reset()
    {
        TestUserRole = "User";
        CustomUserId = null;
        CustomUserEmail = null;
    }
}
