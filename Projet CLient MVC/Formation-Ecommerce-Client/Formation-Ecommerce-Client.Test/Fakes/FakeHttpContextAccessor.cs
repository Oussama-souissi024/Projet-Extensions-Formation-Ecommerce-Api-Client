using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;

namespace Formation_Ecommerce_Client.Test.Fakes;

/// <summary>
/// FakeHttpContextAccessor : simule le HttpContext avec Session pour les tests.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Les services Client MVC accèdent au token JWT via HttpContext.Session.
/// - Pour les tests, on simule un contexte avec ou sans token.
/// 
/// Utilisation :
/// <code>
/// var fakeContext = new FakeHttpContextAccessor();
/// fakeContext.SetSessionValue("JwtToken", "fake-jwt-token");
/// var service = new ProductApiService(fakeFactory, fakeContext);
/// </code>
/// </remarks>
public class FakeHttpContextAccessor : IHttpContextAccessor
{
    private readonly FakeHttpContext _context;

    public FakeHttpContextAccessor()
    {
        _context = new FakeHttpContext();
    }

    public HttpContext? HttpContext
    {
        get => _context;
        set { /* Ignore set */ }
    }

    /// <summary>
    /// Définit une valeur dans la session.
    /// </summary>
    public void SetSessionValue(string key, string value)
    {
        _context.FakeSession.SetString(key, value);
    }

    /// <summary>
    /// Récupère une valeur de la session.
    /// </summary>
    public string? GetSessionValue(string key)
    {
        return _context.FakeSession.GetString(key);
    }

    /// <summary>
    /// Efface la session.
    /// </summary>
    public void ClearSession()
    {
        _context.FakeSession.Clear();
    }
}

/// <summary>
/// Fake HttpContext minimal pour les tests.
/// </summary>
public class FakeHttpContext : HttpContext
{
    private readonly FakeSession _session;
    private ClaimsPrincipal _user;
    private readonly IFeatureCollection _features;

    public FakeHttpContext()
    {
        _session = new FakeSession();
        _user = new ClaimsPrincipal();
        _features = new FeatureCollection();
    }

    public FakeSession FakeSession => _session;

    public override ISession Session 
    { 
        get => _session; 
        set { /* Ignore */ } 
    }

    public override IFeatureCollection Features => _features;
    public override HttpRequest Request => null!;
    public override HttpResponse Response => null!;
    public override ConnectionInfo Connection => null!;
    public override WebSocketManager WebSockets => null!;
    
    public override ClaimsPrincipal User 
    { 
        get => _user; 
        set => _user = value; 
    }
    
    public override IDictionary<object, object?> Items { get; set; } = new Dictionary<object, object?>();
    public override IServiceProvider RequestServices { get; set; } = null!;
    public override CancellationToken RequestAborted { get; set; }
    public override string TraceIdentifier { get; set; } = Guid.NewGuid().ToString();
    public override void Abort() { }
}

/// <summary>
/// Fake Session en mémoire.
/// </summary>
public class FakeSession : ISession
{
    private readonly Dictionary<string, byte[]> _store = new();

    public string Id => "fake-session-id";
    public bool IsAvailable => true;
    public IEnumerable<string> Keys => _store.Keys;

    public void Clear() => _store.Clear();

    public Task CommitAsync(CancellationToken cancellationToken = default) 
        => Task.CompletedTask;

    public Task LoadAsync(CancellationToken cancellationToken = default) 
        => Task.CompletedTask;

    public void Remove(string key) => _store.Remove(key);

    public void Set(string key, byte[] value) => _store[key] = value;

    public bool TryGetValue(string key, out byte[]? value) 
        => _store.TryGetValue(key, out value);
}

/// <summary>
/// Extensions pour faciliter l'utilisation de la session.
/// </summary>
public static class FakeSessionExtensions
{
    public static void SetString(this FakeSession session, string key, string value)
    {
        session.Set(key, System.Text.Encoding.UTF8.GetBytes(value));
    }

    public static string? GetString(this FakeSession session, string key)
    {
        if (session.TryGetValue(key, out var bytes) && bytes != null)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        return null;
    }
}
