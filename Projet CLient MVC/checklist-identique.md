# ✅ Checklist : Transformer le Client MVC pour Consommer l'API

## 📋 Objectif de la Formation

Cette checklist vous guidera pas à pas pour transformer le projet **Formation-Ecommerce-Client** afin qu'il fonctionne exactement comme le projet MVC original (`Formation-Ecommerce-11-2025`), mais en **consommant les API REST** au lieu d'accéder directement à la base de données.

### 🎯 Architecture Finale

```
┌─────────────────────────────────────────┐
│         CLIENT MVC RAZOR                │
│  (Formation-Ecommerce-Client)           │
│         Port: 5002                      │
└──────────────────┬──────────────────────┘
                   │ HTTP/REST (JSON)
                   ▼
┌─────────────────────────────────────────┐
│         API REST                        │
│  (Formation-Ecommerce-11-2025.API)      │
│         Port: 5001                      │
└─────────────────────────────────────────┘
```

---

## 📁 PHASE 1 : Préparation et Configuration Initiale

### 1.1 Vérifier la structure du projet Client MVC
- [x] Ouvrir le projet `Formation-Ecommerce-Client`
- [x] Vérifier que la structure de dossiers est correcte :
  ```
  Formation-Ecommerce-Client/
  ├── Controllers/
  ├── Models/
  │   ├── ApiDtos/
  │   ├── ApiResponses/
  │   ├── Configuration/
  │   └── ViewModels/
  ├── Services/
  │   ├── Interfaces/
  │   └── Implementations/
  ├── Helpers/
  ├── Views/
  └── wwwroot/
  ```

### 1.2 Configurer appsettings.json
- [x] Ouvrir `appsettings.json`
- [x] Vérifier/ajouter la configuration de l'API :
  ```json
  {
    "ApiSettings": {
      "BaseUrl": "https://localhost:5001/api",
      "Timeout": 30
    },
    "JwtSettings": {
      "CookieName": "JwtToken",
      "RefreshCookieName": "RefreshToken"
    }
  }
  ```

### 1.3 Configurer Program.cs
- [x] Vérifier que `HttpClient` est configuré avec l'URL de l'API
- [x] Vérifier que les services sont injectés correctement
- [x] Vérifier que la Session est activée pour stocker le token JWT

---

## 🔌 PHASE 2 : Créer les Services HTTP (Connexion avec l'API)

> **Concept clé** : Les Services HTTP remplacent l'accès direct à la base de données. Ils appellent l'API REST via HttpClient.

### 2.1 Interface de base (IApiServiceBase.cs)
- [x] Créer/vérifier `Services/Interfaces/IApiServiceBase.cs`
- [x] S'assurer que l'interface contient les méthodes CRUD de base

### 2.2 Service Produits (ProductApiService.cs)
- [x] Créer/vérifier `Services/Implementations/ProductApiService.cs`
- [x] Implémenter les méthodes :
  - [x] `GetAllAsync()` → Appelle `GET /api/products`
  - [x] `GetByIdAsync(id)` → Appelle `GET /api/products/{id}`
  - [x] `CreateAsync(product)` → Appelle `POST /api/products`
  - [x] `UpdateAsync(id, product)` → Appelle `PUT /api/products/{id}`
  - [x] `DeleteAsync(id)` → Appelle `DELETE /api/products/{id}`
  - [x] `GetByCategoryAsync(categoryId)` → Appelle `GET /api/products?categoryId={id}`

### 2.3 Service Catégories (CategoryApiService.cs)
- [x] Créer/vérifier `Services/Implementations/CategoryApiService.cs`
- [x] Implémenter les méthodes :
  - [x] `GetAllAsync()` → Appelle `GET /api/categories`
  - [x] `GetByIdAsync(id)` → Appelle `GET /api/categories/{id}`
  - [x] `CreateAsync(category)` → Appelle `POST /api/categories`
  - [x] `UpdateAsync(id, category)` → Appelle `PUT /api/categories/{id}`
  - [x] `DeleteAsync(id)` → Appelle `DELETE /api/categories/{id}`

### 2.4 Service Authentification (AuthApiService.cs)
- [x] Créer/vérifier `Services/Implementations/AuthApiService.cs`
- [x] Implémenter les méthodes :
  - [x] `LoginAsync(email, password)` → Appelle `POST /api/auth/login`
  - [x] `RegisterAsync(user)` → Appelle `POST /api/auth/register`
  - [x] `LogoutAsync()` → Supprime le token de la session
  - [x] `GetProfileAsync()` → Appelle `GET /api/auth/profile`
  - [x] Stocker le token JWT en Session après connexion

### 2.5 Service Panier (CartApiService.cs)
- [x] Créer/vérifier `Services/Implementations/CartApiService.cs`
- [x] Implémenter les méthodes :
  - [x] `GetCartAsync()` → Appelle `GET /api/cart`
  - [x] `AddToCartAsync(productId, quantity)` → Appelle `POST /api/cart`
  - [x] `UpdateQuantityAsync(itemId, quantity)` → Appelle `PUT /api/cart/{itemId}`
  - [x] `RemoveFromCartAsync(itemId)` → Appelle `DELETE /api/cart/{itemId}`
  - [x] `ClearCartAsync()` → Appelle `DELETE /api/cart`

### 2.6 Service Commandes (OrderApiService.cs)
- [x] Créer/vérifier `Services/Implementations/OrderApiService.cs`
- [x] Implémenter les méthodes :
  - [x] `GetAllOrdersAsync()` → Appelle `GET /api/orders`
  - [x] `GetOrderByIdAsync(id)` → Appelle `GET /api/orders/{id}`
  - [x] `CreateOrderAsync(order)` → Appelle `POST /api/orders`
  - [x] `GetMyOrdersAsync()` → Appelle `GET /api/orders/my`
  - [x] `UpdateOrderStatusAsync(id, status)` → Appelle `PUT /api/orders/{id}/status`

### 2.7 Service Coupons (CouponApiService.cs)
- [x] Créer/vérifier `Services/Implementations/CouponApiService.cs`
- [x] Implémenter les méthodes :
  - [x] `GetAllCouponsAsync()` → Appelle `GET /api/coupons`
  - [x] `GetCouponByCodeAsync(code)` → Appelle `GET /api/coupons/{code}`
  - [x] `ApplyCouponAsync(code)` → Appelle `POST /api/coupons/apply`
  - [x] `ValidateCouponAsync(code)` → Appelle `GET /api/coupons/validate/{code}`

---

## 🎮 PHASE 3 : Créer/Modifier les Controllers

> **Concept clé** : Les Controllers utilisent maintenant les Services HTTP au lieu d'accéder directement aux données.

### 3.1 HomeController
- [x] Vérifier que le `HomeController` affiche les produits populaires via l'API
- [x] Méthodes à implémenter :
  - [x] `Index()` → Affiche la page d'accueil avec produits
  - [x] `ApiError()` → Page d'erreur si l'API est indisponible

### 3.2 ProductController
- [x] Créer/vérifier le `ProductController`
- [x] Injecter `IProductApiService` et `ICategoryApiService` dans le constructeur
- [x] Méthodes à implémenter :
  - [x] `Index()` → Liste tous les produits
  - [x] `Details(id)` → Affiche les détails d'un produit
  - [x] `Create()` GET → Affiche le formulaire de création
  - [x] `Create(model)` POST → Envoie le produit à l'API
  - [x] `Edit(id)` GET → Affiche le formulaire de modification
  - [x] `Edit(id, model)` POST → Met à jour via l'API
  - [x] `Delete(id)` GET → Affiche la confirmation
  - [x] `DeleteConfirmed(id)` POST → Supprime via l'API

### 3.3 CategoryController
- [x] Créer/vérifier le `CategoryController`
- [x] Injecter `ICategoryApiService` dans le constructeur
- [x] Méthodes à implémenter :
  - [x] `Index()` → Liste toutes les catégories
  - [x] `Create()` GET/POST → Créer une catégorie
  - [x] `Edit(id)` GET/POST → Modifier une catégorie
  - [x] `Delete(id)` GET/POST → Supprimer une catégorie

### 3.4 AuthController
- [x] Créer/vérifier le `AuthController`
- [x] Injecter `IAuthApiService` dans le constructeur
- [x] Méthodes à implémenter :
  - [x] `Login()` GET → Affiche le formulaire de connexion
  - [x] `Login(model)` POST → Authentifie via l'API et stocke le token
  - [x] `Register()` GET → Affiche le formulaire d'inscription
  - [x] `Register(model)` POST → Inscrit via l'API
  - [x] `Logout()` POST → Supprime le token de la session
  - [x] `Profile()` → Affiche le profil utilisateur
  - [x] `AccessDenied()` → Page accès refusé

### 3.5 CartController
- [x] Créer/vérifier le `CartController`
- [x] Injecter `ICartApiService` dans le constructeur
- [x] Méthodes à implémenter :
  - [x] `Index()` → Affiche le panier
  - [x] `AddToCart(productId, quantity)` POST → Ajoute au panier
  - [x] `UpdateQuantity(itemId, quantity)` POST → Met à jour la quantité
  - [x] `RemoveItem(itemId)` POST → Retire un article
  - [x] `Clear()` POST → Vide le panier
  - [x] `Checkout()` GET → Page de validation

### 3.6 OrderController
- [x] Créer/vérifier le `OrderController`
- [x] Injecter `IOrderApiService` dans le constructeur
- [x] Méthodes à implémenter :
  - [x] `Index()` → Liste les commandes (admin) ou mes commandes (user)
  - [x] `Details(id)` → Affiche les détails d'une commande
  - [x] `Create()` POST → Crée une commande depuis le panier
  - [x] `UpdateStatus(id, status)` POST → Met à jour le statut (admin)

### 3.7 CouponController
- [x] Créer/vérifier le `CouponController`
- [x] Injecter `ICouponApiService` dans le constructeur
- [x] Méthodes à implémenter :
  - [x] `Index()` → Liste les coupons (admin)
  - [x] `Create()` GET/POST → Créer un coupon
  - [x] `Edit(id)` GET/POST → Modifier un coupon
  - [x] `Delete(id)` GET/POST → Supprimer un coupon
  - [x] `Apply(code)` POST → Appliquer un coupon au panier

---

## 🎨 PHASE 4 : Créer/Vérifier les Vues Razor

> **Concept clé** : Les vues restent similaires au MVC original, mais utilisent les ViewModels adaptés pour l'API.

### 4.1 Vues Produits (Views/Product/)
- [x] `Index.cshtml` → Liste des produits avec pagination
- [x] `Details.cshtml` → Détails d'un produit + bouton "Ajouter au panier"
- [x] `Create.cshtml` → Formulaire de création avec upload d'image
- [x] `Edit.cshtml` → Formulaire de modification
- [x] `Delete.cshtml` → Confirmation de suppression

### 4.2 Vues Catégories (Views/Category/)
- [x] `Index.cshtml` → Liste des catégories
- [x] `Create.cshtml` → Formulaire de création
- [x] `Edit.cshtml` → Formulaire de modification
- [x] `Delete.cshtml` → Confirmation de suppression

### 4.3 Vues Authentification (Views/Auth/)
- [x] `Login.cshtml` → Formulaire de connexion
- [x] `Register.cshtml` → Formulaire d'inscription
- [x] `Profile.cshtml` → Page profil utilisateur
- [x] `AccessDenied.cshtml` → Page accès refusé

### 4.4 Vues Panier (Views/Cart/)
- [x] `Index.cshtml` → Affichage du panier avec total
- [x] `Checkout.cshtml` → Page de validation de commande
- [x] `_CartSummary.cshtml` → Partial view pour le résumé

### 4.5 Vues Commandes (Views/Order/)
- [x] `Index.cshtml` → Liste des commandes
- [x] `Details.cshtml` → Détails d'une commande
- [x] `Confirmation.cshtml` → Confirmation après commande

### 4.6 Vues Coupons (Views/Coupon/)
- [x] `Index.cshtml` → Liste des coupons (admin)
- [x] `Create.cshtml` → Formulaire de création
- [x] `Edit.cshtml` → Formulaire de modification

### 4.7 Vues Partagées (Views/Shared/)
- [x] `_Layout.cshtml` → Layout principal avec :
  - [x] Menu de navigation responsive
  - [x] Affichage conditionnel connexion/déconnexion
  - [x] Icône panier avec compteur
  - [x] Messages d'alerte (TempData)
- [x] `_ValidationScriptsPartial.cshtml` → Scripts de validation
- [x] `Error.cshtml` → Page d'erreur générique
- [x] `_LoginPartial.cshtml` → Partial pour le menu utilisateur

---

## 🔐 PHASE 5 : Gestion de l'Authentification JWT

### 5.1 Stockage du Token
- [x] Après connexion réussie, stocker le token dans la Session :
  ```csharp
  HttpContext.Session.SetString("JwtToken", tokenResponse.AccessToken);
  ```

### 5.2 Envoi du Token avec chaque requête API
- [x] Dans chaque Service HTTP, ajouter le token à l'en-tête :
  ```csharp
  _httpClient.DefaultRequestHeaders.Authorization = 
      new AuthenticationHeaderValue("Bearer", token);
  ```

### 5.3 Vérification de l'authentification
- [x] Créer/vérifier `Helpers/AuthorizeApiAttribute.cs`
- [x] Appliquer l'attribut sur les actions nécessitant une authentification :
  ```csharp
  [AuthorizeApi]
  public async Task<IActionResult> Profile()
  ```

### 5.4 Gestion des tokens expirés
- [x] Détecter les erreurs 401 (Unauthorized) de l'API
- [x] Rediriger vers la page de connexion si le token est expiré
- [x] Afficher un message explicatif

---

## ⚠️ PHASE 6 : Gestion des Erreurs

### 6.1 Erreurs réseau
- [x] Capturer les `HttpRequestException` dans les Controllers
- [x] Afficher un message d'erreur approprié à l'utilisateur
- [x] Exemple :
  ```csharp
  try
  {
      var products = await _productService.GetAllAsync();
      return View(products);
  }
  catch (HttpRequestException)
  {
      TempData["Error"] = "Impossible de se connecter au serveur. Veuillez réessayer.";
      return View(new List<ProductViewModel>());
  }
  ```

### 6.2 Erreurs de validation
- [x] Afficher les erreurs retournées par l'API
- [x] Utiliser `ModelState.AddModelError()` pour afficher dans les vues

### 6.3 Page d'erreur API
- [x] Créer `Views/Home/ApiError.cshtml`
- [x] Afficher un message clair si l'API est indisponible

---

## 📦 PHASE 7 : Modèles et DTOs

### 7.1 Modèles de réponse API
- [x] `Models/ApiResponses/ApiResponse.cs` → Réponse générique de l'API
  ```csharp
  public class ApiResponse<T>
  {
      public bool Success { get; set; }
      public string Message { get; set; }
      public T Data { get; set; }
      public List<string> Errors { get; set; }
  }
  ```

### 7.2 DTOs pour l'API
- [x] `Models/ApiDtos/ProductDto.cs`
- [x] `Models/ApiDtos/CategoryDto.cs`
- [x] `Models/ApiDtos/UserDto.cs`
- [x] `Models/ApiDtos/CartDto.cs`
- [x] `Models/ApiDtos/OrderDto.cs`
- [x] `Models/ApiDtos/CouponDto.cs`

### 7.3 ViewModels pour les vues
- [x] `Models/ViewModels/Products/ProductViewModel.cs`
- [x] `Models/ViewModels/Products/CreateProductViewModel.cs`
- [x] `Models/ViewModels/Products/EditProductViewModel.cs`
- [x] `Models/ViewModels/Auth/LoginViewModel.cs`
- [x] `Models/ViewModels/Auth/RegisterViewModel.cs`
- [x] `Models/ViewModels/Cart/CartViewModel.cs`
- [x] `Models/ViewModels/Orders/OrderViewModel.cs`

---

## 🧪 PHASE 8 : Tests et Validation

### 8.1 Démarrer l'API
- [x] Ouvrir un terminal dans le dossier de l'API :
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet API\Formation-Ecommerce-11-2025.API"
  dotnet run
  ```
- [x] Vérifier que l'API démarre sur `https://localhost:5001`
- [x] Tester l'accès à Swagger : `https://localhost:5001/swagger`

### 8.2 Démarrer le Client MVC
- [x] Ouvrir un second terminal :
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet CLient MVC\Formation-Ecommerce-Client"
  dotnet run
  ```
- [x] Vérifier que le client démarre sur `https://localhost:5002`

### 8.3 Tests fonctionnels

#### Authentification
- [x] Inscription d'un nouvel utilisateur
- [x] Connexion avec le compte créé
- [x] Vérification que le menu change après connexion
- [x] Déconnexion

#### Produits
- [x] Afficher la liste des produits
- [x] Voir les détails d'un produit
- [x] Créer un nouveau produit (admin)
- [x] Modifier un produit existant (admin)
- [x] Supprimer un produit (admin)

#### Catégories
- [x] Afficher la liste des catégories
- [x] Créer une nouvelle catégorie (admin)
- [x] Modifier une catégorie (admin)
- [x] Supprimer une catégorie (admin)

#### Panier
- [x] Ajouter un produit au panier
- [x] Modifier la quantité d'un article
- [x] Supprimer un article du panier
- [x] Vider le panier

#### Commandes
- [x] Passer une commande depuis le panier
- [x] Voir l'historique de ses commandes
- [x] Voir les détails d'une commande
- [x] Modifier le statut d'une commande (admin)

#### Coupons
- [x] Créer un coupon de réduction (admin)
- [x] Appliquer un coupon au panier
- [x] Vérifier la réduction appliquée

---

## 🚀 PHASE 9 : Comparaison avec le MVC Original

### Différences clés

| Aspect | MVC Original | Client MVC + API |
|--------|-------------|------------------|
| Accès données | Direct (EF Core) | Via HTTP (API REST) |
| Authentification | Identity (Cookie) | JWT Token |
| Session | Base de données | Token en Session |
| Réutilisabilité | Limité au web | API réutilisable |

### Points à retenir

1. **Le flux de données change** :
   - **Avant** : Controller → Service → Repository → Database
   - **Après** : Controller → Service HTTP → API → Database

2. **Avantages de l'architecture** :
   - API réutilisable (mobile, SPA, autres clients)
   - Séparation des responsabilités
   - Scalabilité indépendante

3. **Points d'attention** :
   - Gestion du token JWT
   - Latence réseau supplémentaire
   - Gestion des erreurs réseau

---

## ⏱️ Estimation du Temps

| Phase | Durée |
|-------|-------|
| Phase 1 : Configuration | 30 min |
| Phase 2 : Services HTTP | 2h |
| Phase 3 : Controllers | 2h |
| Phase 4 : Vues | 2h |
| Phase 5-6 : Auth & Erreurs | 1h |
| Phase 7 : Modèles | 1h |
| Phase 8 : Tests | 1h |
| **TOTAL** | **~10 heures** |

---

## 💡 Script de Démarrage Rapide

Utilisez le fichier `start-dev.bat` pour démarrer les deux projets simultanément :

```batch
@echo off
echo ================================
echo Démarrage de l'environnement de développement
echo ================================

echo Démarrage de l'API...
start cmd /k "cd /d C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet API\Formation-Ecommerce-11-2025.API && dotnet run"

echo Attente de 5 secondes...
timeout /t 5 /nobreak

echo Démarrage du Client MVC...
start cmd /k "cd /d C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet CLient MVC\Formation-Ecommerce-Client && dotnet run"

echo ================================
echo API       : https://localhost:5001
echo Client    : https://localhost:5002
echo Swagger   : https://localhost:5001/swagger
echo ================================
```

---

## 📚 Ressources Complémentaires

- [Documentation HttpClient ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/http-requests)
- [IHttpClientFactory](https://docs.microsoft.com/dotnet/core/extensions/httpclient-factory)
- [Authentification JWT](https://jwt.io/introduction)
- [REST API Best Practices](https://restfulapi.net/)

---

**🎓 Bon apprentissage !**
