using System.Net;
using System.Text.Json;

namespace Formation_Ecommerce_Client.Test.Fakes;

/// <summary>
/// FakeHttpMessageHandler : simule les réponses HTTP de l'API pour les tests unitaires.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Dans une architecture Client MVC → API, les services utilisent HttpClient.
/// - Pour tester ces services sans appeler la vraie API, on mock le HttpMessageHandler.
/// - Cela permet de contrôler les réponses HTTP (status, body) pour chaque test.
/// 
/// Utilisation :
/// <code>
/// var handler = new FakeHttpMessageHandler();
/// handler.SetupResponse(HttpStatusCode.OK, new { Data = products });
/// var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
/// </code>
/// </remarks>
public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly List<HttpRequestMessage> _capturedRequests = new();
    private HttpStatusCode _statusCode = HttpStatusCode.OK;
    private string _responseContent = "{}";
    private Func<HttpRequestMessage, HttpResponseMessage>? _customHandler;

    /// <summary>
    /// Liste des requêtes capturées pour vérification dans les tests.
    /// </summary>
    public IReadOnlyList<HttpRequestMessage> CapturedRequests => _capturedRequests;

    /// <summary>
    /// Dernière requête envoyée.
    /// </summary>
    public HttpRequestMessage? LastRequest => _capturedRequests.LastOrDefault();

    /// <summary>
    /// Configure une réponse fixe pour toutes les requêtes.
    /// </summary>
    public void SetupResponse(HttpStatusCode statusCode, string content)
    {
        _statusCode = statusCode;
        _responseContent = content;
        _customHandler = null;
    }

    /// <summary>
    /// Configure une réponse JSON sérialisée.
    /// </summary>
    public void SetupResponse<T>(HttpStatusCode statusCode, T data)
    {
        _statusCode = statusCode;
        _responseContent = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        _customHandler = null;
    }

    /// <summary>
    /// Configure un handler personnalisé pour des réponses conditionnelles.
    /// </summary>
    public void SetupCustomHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _customHandler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _capturedRequests.Add(request);

        HttpResponseMessage response;

        if (_customHandler != null)
        {
            response = _customHandler(request);
        }
        else
        {
            response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseContent, System.Text.Encoding.UTF8, "application/json")
            };
        }

        response.RequestMessage = request;
        return Task.FromResult(response);
    }

    /// <summary>
    /// Réinitialise le handler entre les tests.
    /// </summary>
    public void Reset()
    {
        _capturedRequests.Clear();
        _statusCode = HttpStatusCode.OK;
        _responseContent = "{}";
        _customHandler = null;
    }
}
