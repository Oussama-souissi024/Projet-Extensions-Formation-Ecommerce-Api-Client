namespace Formation_Ecommerce_Client.Helpers
{
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
