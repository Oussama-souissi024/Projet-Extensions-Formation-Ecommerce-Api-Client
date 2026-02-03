namespace Formation_Ecommerce_Client.Helpers
{
    /// <summary>
    /// Middleware côté Client MVC : intercepte certaines exceptions liées aux appels HTTP vers l'API (ex: API indisponible).
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Dans le monolithe, une erreur pouvait venir directement du serveur MVC (même process).
    /// - Ici, l'UI dépend d'un serveur API externe : un échec réseau (HttpRequestException) doit être géré explicitement.
    /// - Le middleware redirige vers une page d'erreur UI avec un message adapté à l'utilisateur.
    /// </remarks>
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpRequestException)
            {
                // Rediriger vers une page d'erreur ou afficher un message
                // Pour l'instant on redirige vers Home avec un paramètre d'erreur
                 context.Response.Redirect("/Home/Error?message=" + Uri.EscapeDataString("Erreur de communication avec l'API."));
            }
        }
    }
}
