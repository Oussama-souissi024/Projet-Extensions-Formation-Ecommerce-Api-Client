# Tests du projet Client MVC

Ce projet contient les tests automatisés pour l'application **Formation-Ecommerce-Client**.

## Structure du projet

```
Formation-Ecommerce-Client.Test/
├── Fakes/                          # Doublures de tests HTTP
│   ├── FakeHttpMessageHandler.cs   # Mock des réponses HTTP
│   ├── FakeHttpClientFactory.cs    # Factory pour injecter les fakes
│   └── FakeHttpContextAccessor.cs  # Mock HttpContext + Session
│
├── Common/                         # Helpers partagés
│   ├── ValidationHelper.cs         # Validation DataAnnotations
│   └── ApiResponseBuilder.cs       # Construction des réponses API
│
├── Unit/                           # Tests unitaires
│   ├── ProductApiServiceTests.cs   # Tests du service HTTP produits
│   └── ViewModelValidationTests.cs # Tests des ViewModels
│
└── Integration/                    # Tests d'intégration
    ├── CustomWebApplicationFactory.cs
    └── ProductControllerTests.cs
```

## Exécution des tests

```powershell
# Tous les tests
cd Formation-Ecommerce-Client
dotnet test Formation-Ecommerce-Client.sln

# Tests avec détails
dotnet test --verbosity normal

# Uniquement tests unitaires
dotnet test --filter "FullyQualifiedName~Unit"

# Uniquement tests intégration
dotnet test --filter "FullyQualifiedName~Integration"
```

## Points pédagogiques clés

### Différence avec les tests API

| Aspect | Tests API | Tests Client MVC |
|--------|-----------|------------------|
| Mock | Repositories | HttpClient |
| Dépendance | Base de données | API REST |
| Handler | TestAuthHandler | FakeHttpMessageHandler |

### FakeHttpMessageHandler

Le `FakeHttpMessageHandler` permet de contrôler les réponses HTTP :

```csharp
var handler = new FakeHttpMessageHandler();
handler.SetupResponse(HttpStatusCode.OK, "{...json...}");

var factory = new FakeHttpClientFactory(handler);
var service = new ProductApiService(factory, contextAccessor);
```

### Session et JWT

Le `FakeHttpContextAccessor` simule la session avec le token JWT :

```csharp
var contextAccessor = new FakeHttpContextAccessor();
contextAccessor.SetSessionValue("JwtToken", "fake-token");
```
