# ✅ Checklist — Mettre en place un projet de tests pour l’API (xUnit + Integration)

> Objectif : créer une “couche de tests” similaire à `Formation-Ecommerce-11-2025.Test` (projet MVC), mais adaptée à la solution **API** :
> `Projet extensions/Projet API/Formations-Ecommerce-API.sln`
>
> Types de tests visés :
> - **Tests unitaires** (services de la couche Application, via fakes)
> - **Tests d’intégration API** (endpoints REST via `WebApplicationFactory<Program>`)
>
---

## 0) Pré-requis

- [ ] Avoir le **.NET SDK 8** installé
- [ ] Pouvoir accéder à NuGet (`https://api.nuget.org`) (sinon `dotnet restore` échoue)
- [ ] Vérifier que le projet API expose bien :
  - [x] `public partial class Program { }` dans `Formation-Ecommerce-11-2025.API/Program.cs` (nécessaire pour `WebApplicationFactory`)

---

## 1) Créer le projet de tests

### 1.1 Créer un projet xUnit (net8.0)

- [ ] Ouvrir un terminal dans :
  - `Projet extensions/Projet API`
- [ ] Créer le projet de tests :

```bash
# exemple de nom (tu peux aussi choisir ...API.Tests)
dotnet new xunit -n Formation-Ecommerce-11-2025.API.Test -f net8.0
```

### 1.2 Ajouter le projet de tests à la solution

- [ ] Ajouter au `.sln` :

```bash
dotnet sln .\Formation-Ecommerce-API.sln add .\Formation-Ecommerce-11-2025.API.Test\Formation-Ecommerce-11-2025.API.Test.csproj
```

### 1.3 Références de projets (ProjectReference)

Dans `Formation-Ecommerce-11-2025.API.Test.csproj` :

- [ ] Référencer le projet API (obligatoire pour `WebApplicationFactory<Program>`) :
  - `..\Formation-Ecommerce-11-2025.API\Formation-Ecommerce-11-2025.API.csproj`
- [ ] Référencer aussi (optionnel mais pratique) :
  - `..\Formation-Ecommerce-11-2025.Application\...`
  - `..\Formation-Ecommerce-11-2025.Core\...`
  - `..\Formation-Ecommerce-11-2025.Infrastructure\...`

---

## 2) Ajouter les packages NuGet “comme dans le projet MVC Test”

### 2.1 Packages essentiels (intégration)

- [ ] Installer :

```bash
dotnet add .\Formation-Ecommerce-11-2025.API.Test\Formation-Ecommerce-11-2025.API.Test.csproj package Microsoft.AspNetCore.Mvc.Testing --version 8.0.11
dotnet add .\Formation-Ecommerce-11-2025.API.Test\Formation-Ecommerce-11-2025.API.Test.csproj package Microsoft.EntityFrameworkCore.InMemory --version 8.0.11
```

### 2.2 Packages optionnels (qualité)

- [ ] (Optionnel) assertions plus lisibles :

```bash
dotnet add .\Formation-Ecommerce-11-2025.API.Test\Formation-Ecommerce-11-2025.API.Test.csproj package FluentAssertions
```

- [ ] (Optionnel) couverture de code :

```bash
dotnet add .\Formation-Ecommerce-11-2025.API.Test\Formation-Ecommerce-11-2025.API.Test.csproj package coverlet.collector
```

---

## 3) Créer la structure de dossiers de tests

Dans `Formation-Ecommerce-11-2025.API.Test/` :

- [ ] Créer les dossiers :
  - `Integration/`
  - `Unit/`
  - `Fakes/`
  - `Common/`

---

## 4) Intégration : CustomWebApplicationFactory (remplacer SQL Server par InMemory)

> Le but est d’éviter une DB SQL Server en tests CI/locaux : on lance l’API **en mémoire**, mais avec une DB **EF Core InMemory**.

### 4.1 Créer `Integration/CustomWebApplicationFactory.cs`

- [ ] Créer une classe :
  - `public class CustomWebApplicationFactory : WebApplicationFactory<Program>`
- [ ] Override `ConfigureWebHost` pour :
  - [ ] Retirer `DbContextOptions<ApplicationDbContext>` enregistré par l’API
  - [ ] Reconfigurer `ApplicationDbContext` avec InMemory
  - [ ] (Recommandé) Appliquer `EnsureCreated()`

Pseudo-checklist :

- [ ] `builder.ConfigureServices(services => { ... })`
- [ ] `services.Remove(...)` l’ancien DbContext
- [ ] `services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase("TestDb_" + Guid.NewGuid()))`
- [ ] Construire un scope, récupérer `ApplicationDbContext`, faire `Database.EnsureCreated()`

### 4.2 Gérer les dépendances externes (email)

Ton API utilise `IEmailSender` + `EmailSettings`. En tests, tu ne veux **jamais** envoyer un email.

- [ ] Dans `CustomWebApplicationFactory`, remplacer `IEmailSender` par une fake :
  - [ ] `services.AddSingleton<IEmailSender, FakeEmailSender>()`

- [ ] Créer `Fakes/FakeEmailSender.cs` qui :
  - [ ] implémente `IEmailSender`
  - [ ] ne fait rien (No-Op)

---

## 5) Intégration : gérer l’authentification JWT dans les tests

Plusieurs endpoints ont `[Authorize]` (ex : `POST api/Products`).

### Option A (la plus simple) — Créer un schéma d’authentification “Test”

- [ ] Créer un handler `TestAuthHandler` dans `Common/` qui renvoie toujours un utilisateur authentifié
- [ ] Dans `CustomWebApplicationFactory`, remplacer l’auth en tests :
  - [ ] `services.AddAuthentication("Test")...`
  - [ ] `services.AddAuthorization()`
  - [ ] Forcer `DefaultAuthenticateScheme` et `DefaultChallengeScheme` à `Test`

Avantage : pas besoin de générer de vrais JWT.

### Option B — Tester un vrai login + récupérer un JWT

- [ ] Écrire un test qui :
  - [ ] `POST api/Auth/register`
  - [ ] confirmer email (si nécessaire selon logique)
  - [ ] `POST api/Auth/login` et récupérer `token`
  - [ ] ajouter `Authorization: Bearer <token>` sur les requêtes suivantes

Avantage : workflow réaliste, mais plus long (et dépend des règles de confirmation email).

---

## 6) Écrire des tests d’intégration “smoke tests”

### 6.1 Tests simples (sans auth)

- [ ] `GET /api/Products` doit retourner `200 OK`
- [ ] `GET /api/Categories` doit retourner `200 OK`

### 6.2 Tests d’erreurs

- [ ] `GET /api/Products/{id}` avec un GUID inexistant doit retourner `404 NotFound`
- [ ] `POST /api/Categories` sans body / body invalide doit retourner `400 BadRequest`

### 6.3 Tests avec auth

- [ ] `POST /api/Products` sans token doit retourner `401 Unauthorized`
- [ ] `POST /api/Products` avec auth “Test” (Option A) doit retourner `201 Created`

---

## 7) Écrire des tests unitaires (logique Application)

> Même stratégie que ton projet MVC Test : isoler les services avec des fakes (repositories/helpers).

- [ ] Identifier 1 service simple (ex: `CategoryService`, `CouponService`, etc.)
- [ ] Créer des fakes repository en mémoire dans `Fakes/`
- [ ] Écrire des tests unitaires :
  - [ ] Create (retourne item)
  - [ ] ReadAll
  - [ ] ReadById (ok + erreur)
  - [ ] Update
  - [ ] Delete

---

## 8) Exécuter les tests

- [ ] Depuis la racine :

```bash
dotnet test .\Formation-Ecommerce-API.sln
```

- [ ] Ou seulement le projet de tests :

```bash
dotnet test .\Formation-Ecommerce-11-2025.API.Test\Formation-Ecommerce-11-2025.API.Test.csproj
```

- [ ] Filtrer :

```bash
dotnet test --filter "FullyQualifiedName~Integration"
dotnet test --filter "FullyQualifiedName~Unit"
```

---

## 9) Checklist Git (important)

- [ ] Ne jamais commiter de secrets dans `appsettings.json`
- [ ] En tests, privilégier :
  - [ ] `WebApplicationFactory` + `InMemoryDatabase`
  - [ ] fake `IEmailSender`
  - [ ] auth “Test”

---

## 10) Résultat attendu

- [ ] La solution `Formation-Ecommerce-API.sln` contient un projet de tests `Formation-Ecommerce-11-2025.API.Test`
- [ ] Les tests d’intégration lancent l’API en mémoire (pas de SQL Server requis)
- [ ] Les endpoints principaux ont des smoke tests + tests d’erreurs + tests auth
- [ ] Les services Application ont quelques tests unitaires (fakes)
