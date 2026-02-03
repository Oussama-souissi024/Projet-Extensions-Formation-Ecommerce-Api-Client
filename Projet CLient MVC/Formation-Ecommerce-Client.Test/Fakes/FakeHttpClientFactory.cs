namespace Formation_Ecommerce_Client.Test.Fakes;

/// <summary>
/// FakeHttpClientFactory : factory qui retourne un HttpClient avec le FakeHttpMessageHandler.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Les services Client MVC utilisent IHttpClientFactory pour créer des HttpClients.
/// - Pour les tests, on injecte cette fake factory qui retourne un client contrôlé.
/// 
/// Utilisation :
/// <code>
/// var fakeHandler = new FakeHttpMessageHandler();
/// var fakeFactory = new FakeHttpClientFactory(fakeHandler, "http://api.test/");
/// var service = new ProductApiService(fakeFactory, fakeContextAccessor);
/// </code>
/// </remarks>
public class FakeHttpClientFactory : IHttpClientFactory
{
    private readonly FakeHttpMessageHandler _handler;
    private readonly string _baseAddress;
    private readonly Dictionary<string, HttpClient> _clients = new();

    public FakeHttpClientFactory(FakeHttpMessageHandler handler, string baseAddress = "http://test-api/api/")
    {
        _handler = handler;
        _baseAddress = baseAddress;
    }

    /// <summary>
    /// Le FakeHttpMessageHandler utilisé par les clients créés.
    /// </summary>
    public FakeHttpMessageHandler Handler => _handler;

    public HttpClient CreateClient(string name)
    {
        if (!_clients.TryGetValue(name, out var client))
        {
            client = new HttpClient(_handler, disposeHandler: false)
            {
                BaseAddress = new Uri(_baseAddress)
            };
            _clients[name] = client;
        }
        return client;
    }
}
