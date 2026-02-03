using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace Formation_Ecommerce_Client.Helpers
{
    /// <summary>
    /// Filtre MVC côté Client : vérifie la présence/validité du JWT en session et hydrate le <see cref="System.Security.Claims.ClaimsPrincipal"/>.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Dans le monolithe, l'authentification est souvent basée sur cookies Identity et <c>User</c> est alimenté automatiquement.
    /// - Ici, le Client MVC n'est pas la source de vérité : il stocke un JWT obtenu depuis l'API et doit reconstruire l'identité.
    /// - Cette étape permet d'utiliser <c>User.IsInRole(...)</c> dans les vues Razor (menus, boutons), sans remplacer la sécurité API.
    /// </remarks>
    public class AuthorizeApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("JwtToken");
            var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl });
                return;
            }

            // Vérifier si le token est expiré
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Vérifier si le token est expiré
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                context.HttpContext.Session.Remove("JwtToken");
                context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl });
                return;
            }

            // Hydrater l'utilisateur (User/ClaimsPrincipal) pour que User.IsInRole fonctionne dans les Vues
            // On détecte automatiquement si le token utilise les noms courts ("role") ou les URI complets (ClaimTypes.Role)
            var roleClaimType = jwtToken.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.Role) 
                ? System.Security.Claims.ClaimTypes.Role 
                : "role";

            var nameClaimType = jwtToken.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.Name) 
                ? System.Security.Claims.ClaimTypes.Name 
                : "unique_name";

            var identity = new System.Security.Claims.ClaimsIdentity(jwtToken.Claims, "Jwt", nameClaimType, roleClaimType);
            context.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(identity);

            base.OnActionExecuting(context);
        }
    }
}
