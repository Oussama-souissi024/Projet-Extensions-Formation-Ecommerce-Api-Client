# Formation-Ecommerce-11-2025.Test

Projet de tests automatisés (xUnit) pour l'API E-Commerce.

## 📁 Structure du projet

```
Formation-Ecommerce-11-2025.Test/
├── Unit/                          # Tests unitaires
│   ├── ProductServicesTests.cs    # Tests du service produits
│   ├── DtoValidationTests.cs      # Tests de validation des DTOs
│   └── AutoMapperProfileTests.cs  # Tests des profils AutoMapper
│
├── Integration/                   # Tests d'intégration HTTP
│   ├── CustomWebApplicationFactory.cs  # Factory de test
│   ├── ProductsEndpointTests.cs   # Tests endpoints Products
│   ├── CategoriesEndpointTests.cs # Tests endpoints Categories
│   └── AuthorizedEndpointTests.cs # Tests endpoints protégés
│
├── Fakes/                         # Implémentations mémoire
│   ├── FakeProductRepository.cs   # Fake du repository produits
│   ├── FakeCategoryRepository.cs  # Fake du repository catégories
│   ├── FakeCartRepository.cs      # Fake du repository panier
│   ├── FakeFileHelper.cs          # Fake de l'helper fichiers
│   └── FakeCategoryService.cs     # Fake du service catégories
│
└── Common/                        # Helpers partagés
    ├── ValidationHelper.cs        # Validation DataAnnotations
    └── TestAuthHandler.cs         # Handler d'auth de test JWT
```

## 🚀 Exécution des tests

```powershell
# Depuis le dossier de la solution
cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet full stack MVC\Projet extensions\Projet API"

# Exécuter tous les tests
dotnet test Formation-Ecommerce-API.sln

# Exécuter avec plus de détails
dotnet test Formation-Ecommerce-API.sln --verbosity normal

# Exécuter uniquement les tests unitaires
dotnet test --filter "FullyQualifiedName~Unit"

# Exécuter uniquement les tests d'intégration
dotnet test --filter "FullyQualifiedName~Integration"
```

## 📊 Couverture de code

```powershell
# Installer l'outil de rapport (une seule fois)
dotnet tool install --global dotnet-reportgenerator-globaltool

# Exécuter avec collecte de couverture
dotnet test --collect:"XPlat Code Coverage"

# Générer le rapport HTML
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html
```

## 🔍 Différences entre tests Unit et Integration

| Aspect | Tests Unitaires | Tests d'Intégration |
|--------|-----------------|---------------------|
| **Vitesse** | Rapides (< 1ms) | Plus lents (démarrage serveur) |
| **Isolation** | Totale (fakes) | Partielle (vraie DI) |
| **Base de données** | Aucune | EF Core InMemory |
| **Serveur HTTP** | Non démarré | Démarré en mémoire |
| **Authentification** | N/A | TestAuthHandler (fake JWT) |

## 🔐 Tests d'authentification

Le projet utilise `TestAuthHandler` pour simuler l'authentification JWT :

```csharp
// Créer un client authentifié en tant qu'Admin
var client = _factory.CreateAuthenticatedClient("Admin");

// Créer un client non authentifié
var client = _factory.CreateUnauthenticatedClient();

// Changer le rôle de l'utilisateur de test
TestAuthHandler.TestUserRole = "User";
```

## 📝 Conventions de nommage

Les tests suivent la convention : `MethodName_Scenario_ExpectedResult`

Exemples :
- `AddAsync_WithImage_UploadsFileAndSetsImageUrl`
- `ReadByIdAsync_WhenNotFound_ThrowsKeyNotFoundException`
- `GetAllProducts_ReturnsOk`

## 🛠️ Packages utilisés

| Package | Version | Rôle |
|---------|---------|------|
| xunit | 2.5.3 | Framework de tests |
| xunit.runner.visualstudio | 2.5.3 | Intégration Visual Studio |
| Microsoft.NET.Test.Sdk | 17.8.0 | SDK de tests .NET |
| Microsoft.AspNetCore.Mvc.Testing | 8.0.11 | WebApplicationFactory |
| Microsoft.EntityFrameworkCore.InMemory | 8.0.11 | Base de données de test |
| coverlet.collector | 6.0.0 | Couverture de code |
