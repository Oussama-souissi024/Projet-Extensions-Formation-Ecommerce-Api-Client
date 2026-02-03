using System.Net;
using System.Text.Json;
using Formation_Ecommerce_11_2025.API.Models;

namespace Formation_Ecommerce_11_2025.API.Middleware
{
    // ==============================================================================================
    // 📘 CONCEPT PÉDAGOGIQUE : Middleware (Intergiciel)
    // ==============================================================================================
    // Un Middleware est comme un "douanier" ou un "filtre" placé à l'entrée et à la sortie de l'API.
    // Chaque requête HTTP doit traverser ce middleware avant d'atteindre le Controller.
    //
    // Ici, nous utilisons un Middleware de "Gestion Globale des Erreurs" (Global Exception Handling).
    //
    // POURQUOI ?
    // Au lieu de mettre des "try-catch" dans chaque action de chaque Controller (ce qui serait répétitif),
    // on centralise la gestion des bugs ici.
    // Si une erreur survient n'importe où dans l'application, elle "remonte" jusqu'ici,
    // et ce middleware la capture pour renvoyer une réponse propre (JSON) au client, jamais de HTML ou de crash.
    // ==============================================================================================
    /// <summary>
    /// Middleware global d'exception : intercepte les exceptions non gérées et renvoie une réponse JSON standardisée.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - En MVC, on affiche souvent une page d'erreur (HTML). En API, on renvoie une erreur structurée (JSON).
    /// - Ce middleware remplace des try/catch répétitifs dans chaque contrôleur.
    /// - Le format de sortie s'appuie sur <see cref="ApiResponse{T}"/> afin que le client (MVC séparé) gère l'erreur de façon uniforme.
    /// </remarks>
    public class GlobalExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // ➡ Passe la main au suivant (Controller, ou autre Middleware)
                await next(context);
            }
            catch (Exception ex)
            {
                // 🛑 Si une erreur "remonte" (n'a pas été gérée plus bas), on l'attrape ici.
                await HandleExceptionAsync(context, ex);
            }
        }

        // Transforme l'exception C# technique (ex: NullReferenceException) en réponse HTTP standardisée (400, 404, 500)
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message), // 404
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message), // 400
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message), // 401
                _ => (HttpStatusCode.InternalServerError, "Une erreur inattendue est survenue") // 500
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;


            // On utilise notre format standard ApiResponse pour que le client comprenne l'erreur
            var payload = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { exception.Message }
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
