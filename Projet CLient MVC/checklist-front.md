# ✅ Checklist Front-End : Finaliser le Projet Client MVC

## 📋 Objectif
Cette checklist guide la finalisation du projet **Formation-Ecommerce-Client** pour qu'il consomme correctement l'API REST tout en offrant exactement la même expérience utilisateur que le projet MVC original.

> 💡 **Note importante** : L'API doit être démarrée sur le même port configuré dans appsettings.json. Par défaut : `http://localhost:5225/api/`

---

## 🎯 Architecture Rappel

```
┌────────────────────────────────────────────────┐
│        CLIENT MVC (Port 5002)                  │
│  ┌──────────────────────────────────────────┐  │
│  │ Views (CSHTML) → Interface Utilisateur  │  │
│  │ Controllers → Orchestration             │  │
│  │ Services HTTP → Appels API              │  │
│  │ ViewModels → Données pour les vues      │  │
│  └──────────────────────────────────────────┘  │
└─────────────────────┬──────────────────────────┘
                      │ HTTP/REST (JSON)
                      │ + JWT Token
                      ▼
┌────────────────────────────────────────────────┐
│        API REST (Port 5001)                    │
│  ┌──────────────────────────────────────────┐  │
│  │ Controllers API → Endpoints REST        │  │
│  │ Application Layer → Use Cases           │  │
│  │ Core Layer → Entités Business           │  │
│  │ Infrastructure → Database Access        │  │
│  └──────────────────────────────────────────┘  │
└────────────────────────────────────────────────┘
```

---

## 📦 PHASE 1 : Audit et Vérification de la Structure

### 1.1 Vérifier la structure des dossiers
- [x] Vérifier que tous les dossiers existent :
  ```
  Formation-Ecommerce-Client/
  ├── Controllers/ ✅
  ├── Services/ ✅
  │   ├── Interfaces/ ✅
  │   └── Implementations/ ✅
  ├── Models/ ✅
  │   ├── ViewModels/ ✅
  │   ├── ApiDtos/ ✅
  │   ├── ApiResponses/ ✅
  │   └── Configuration/ ✅
  ├── Helpers/ ✅
  ├── Views/ ✅
  └── wwwroot/ ✅
  ```

### 1.2 Vérifier les Controllers existants
- [x] Lister tous les Controllers actuels :
  - [x] `HomeController.cs` ✅
  - [x] `AuthController.cs` ✅
  - [x] `ProductController.cs` ✅
  - [x] `CategoryController.cs` ✅
  - [x] `CartController.cs` ✅
  - [x] `OrderController.cs` ✅
  - [x] `CouponController.cs` ✅

### 1.3 Comparer avec le projet MVC original
- [ ] Ouvrir le projet original : `C:\Users\oussa\OneDrive\Desktop\Formation\Projet full stack MVC\Formation-Ecommerce-11-2025`
- [ ] Lister tous les Controllers du projet original
- [ ] Identifier les Controllers manquants dans le Client MVC
- [ ] Identifier les actions manquantes dans chaque Controller

### 1.4 Vérifier les Services HTTP
- [x] Lister tous les services dans `Services/Implementations/`
  - ✅ AuthApiService.cs, CartApiService.cs, CategoryApiService.cs, CouponApiService.cs, OrderApiService.cs, ProductApiService.cs
- [x] Vérifier que chaque service correspond à un Controller ✅
- [x] S'assurer que tous les services implémentent leur interface ✅ (IAuthApiService définie dans AuthApiService.cs)

---

## 🔌 PHASE 2 : Services HTTP - Vérification et Complétion

> 💡 **Note** : Tous les endpoints mentionnés ci-dessous ont été vérifiés dans l'API et correspondent exactement aux endpoints disponibles dans `Formation-Ecommerce-11-2025.API`.


### 2.1 ProductApiService
- [x] Vérifier l'existence de `Services/Implementations/ProductApiService.cs` ✅
- [x] Vérifier les méthodes implémentées :
  - [x] `GetAllAsync()` → GET `/api/products` ✅
  - [x] `GetByIdAsync(id)` → GET `/api/products/{id}` ✅
  - [x] `CreateAsync(model)` → POST `/api/products` (avec multipart/form-data pour l'image) ✅
  - [x] `UpdateAsync(id, model)` → PUT `/api/products/{id}` ✅
  - [x] `DeleteAsync(id)` → DELETE `/api/products/{id}` ✅
  
  > 💡 **Note** : Les endpoints de filtrage par catégorie et de recherche ne sont pas exposés dans l'API actuelle. Si nécessaire, utiliser `GetAllAsync()` et filtrer côté client.
  
- [x] Vérifier que le token JWT est ajouté aux en-têtes HTTP pour les opérations Create, Update, Delete ✅
- [x] Vérifier la gestion des erreurs (try-catch, HttpRequestException) ✅

### 2.2 CategoryApiService
- [x] Vérifier l'existence de `Services/Implementations/CategoryApiService.cs` ✅
- [x] Vérifier les méthodes implémentées :
  - [x] `GetAllAsync()` → GET `/api/categories` ✅
  - [x] `GetByIdAsync(id)` → GET `/api/categories/{id}` ✅
  - [x] `CreateAsync(model)` → POST `/api/categories` ✅
  - [x] `UpdateAsync(id, model)` → PUT `/api/categories/{id}` ✅
  - [x] `DeleteAsync(id)` → DELETE `/api/categories/{id}` ✅

### 2.3 Service Authentification (AuthApiService.cs)
- [x] Créer/vérifier `Services/Implementations/AuthApiService.cs` ✅
- [x] Implémenter les méthodes :
  - [x] `LoginAsync(email, password)` → Appelle `POST /api/auth/login` ✅
    - Reçoit `ApiResponse<JwtLoginResponseDto>` avec `Token`, `Email`, `UserName`, `Roles`, `ExpiresAt`
    - Stocke le token dans la Session
  - [x] `RegisterAsync(user)` → Appelle `POST /api/auth/register` ✅
  - [x] `LogoutAsync()` → Supprime le token de la session ✅
  - [ ] `GetProfileAsync()` → Appelle `GET /api/auth/profile` (si disponible) ⚠️ Non implémenté (endpoint n'existe pas dansl'API)
  - [x] `ConfirmEmailAsync(userId, token)` → Appelle `GET /api/auth/confirm-email?userId={}&token={}` ✅ **AJOUTÉ**
  - [x] `ForgotPasswordAsync(email)` → Appelle `POST /api/auth/forgot-password` ✅
  - [x] `ResetPasswordAsync(userId, token, newPassword)` → Appelle `POST /api/auth/reset-password` ✅
- [x] Vérifier que le token est stocké en Session après connexion ✅
- [x] Vérifier que le token est supprimé après déconnexion ✅

### 2.4 CartApiService
- [x] Vérifier l'existence de `Services/Implementations/CartApiService.cs` ✅
- [x] Vérifier les méthodes implémentées :
  - [x] `GetCartAsync()` → GET `/api/cart` ✅
  - [x] `UpsertCartAsync(productId, quantity)` → POST `/api/cart` (ajouter/mettre à jour) ✅
  - [x] `RemoveItemAsync(cartDetailsId)` → DELETE `/api/cart/items/{cartDetailsId}` ✅
  - [x] `ApplyCouponAsync(couponCode)` → POST `/api/cart/apply-coupon` ✅
  - [x] `RemoveCouponAsync()` → POST `/api/cart/remove-coupon` ✅
  - [x] `ClearCartAsync()` → DELETE `/api/cart` ✅

### 2.5 OrderApiService
- [x] Vérifier l'existence de `Services/Implementations/OrderApiService.cs` ✅
- [x] Vérifier les méthodes implémentées :
  - [x] `GetMyOrdersAsync()` → GET `/api/orders/my` ✅
  - [x] `GetAllOrdersAsync()` → GET `/api/orders` (admin uniquement) ✅
  - [x] `GetOrderByIdAsync(id)` → GET `/api/orders/{id}` ✅
  - [x] `GetOrderDetailsAsync(id)` → GET `/api/orders/{id}/details` ✅
  - [x] `CreateOrderAsync(model)` → POST `/api/orders` ✅
  - [x] `UpdateOrderStatusAsync(id, status)` → PUT `/api/orders/{id}/status` (admin) ✅
  - [x] `CancelOrderAsync(id)` → PUT `/api/orders/{id}/cancel` ✅

### 2.6 CouponApiService
- [x] Vérifier l'existence de `Services/Implementations/CouponApiService.cs` ✅
- [x] Vérifier les méthodes implémentées :
  - [x] `GetAllCouponsAsync()` → GET `/api/coupons` (admin uniquement) ✅
  - [x] `GetCouponByIdAsync(id)` → GET `/api/coupons/{id}` (admin) ✅
  - [x] `ValidateCouponAsync(code)` → GET `/api/coupons/validate/{code}` ✅
  - [x] `CreateCouponAsync(model)` → POST `/api/coupons` (admin) ✅
  - [x] `UpdateCouponAsync(id, model)` → PUT `/api/coupons/{id}` (admin) ✅
  - [x] `DeleteCouponAsync(id)` → DELETE `/api/coupons/{id}` (admin) ✅

---

## 🎮 PHASE 3 : Controllers - Audit et Complétion

### 3.1 HomeController
- [x] Vérifier que le `HomeController` existe ✅
- [x] Vérifier les actions :
  - [x] `Index()` → Affiche la page d'accueil ✅
  - [x] `Privacy()` → Page de confidentialité ✅
  - [x] `ApiError()` → Page d'erreur API indisponible ✅
  - [x] `Error()` → Page d'erreur générique ✅

### 3.2 ProductController
- [x] Vérifier l'injection de `IProductApiService` et `ICategoryApiService` ✅
- [x] Vérifier les actions GET :
  - [x] `Index()` → Liste tous les produits ✅
  - [x] `Details(id)` → Affiche les détails d'un produit ✅ **AJOUTÉ**
  - [x] `Create()` → Affiche le formulaire de création ✅
  - [x] `Edit(id)` → Affiche le formulaire de modification ✅
  - [x] `Delete(id)` → Affiche la confirmation de suppression ✅
- [x] Vérifier les actions POST :
  - [x] `Create(model)` → Envoie le produit à l'API ✅
  - [x] `Edit(id, model)` → Met à jour via l'API ✅
  - [x] `DeleteConfirmed(id)` → Supprime via l'API ✅
- [x] Vérifier la gestion des erreurs (try-catch) ✅
- [x] Vérifier l'affichage des messages TempData (Success/Error) ✅
- [x] Vérifier que les catégories sont chargées pour les dropdowns ✅

### 3.3 CategoryController
- [x] Vérifier l'injection de `ICategoryApiService` ✅
- [x] Vérifier les actions :
  - [x] `Index()` → Liste toutes les catégories ✅
  - [x] `Create()` GET/POST → Créer une catégorie ✅
  - [x] `Edit(id)` GET/POST → Modifier une catégorie ✅
  - [x] `Delete(id)` GET/POST → Supprimer une catégorie ✅

### 3.4 AuthController
- [x] Créer/vérifier le `AuthController` ✅
- [x] Injecter `IAuthApiService` dans le constructeur ✅
- [x] Méthodes à implémenter :
  - [x] `Login()` GET → Affiche le formulaire de connexion ✅
  - [x] `Login(model)` POST → Authentifie via l'API et stocke le token ✅
  - [x] `Register()` GET → Affiche le formulaire d'inscription ✅
  - [x] `Register(model)` POST → Inscrit via l'API ✅
  - [x] `Logout()` POST → Supprime le token de la session ✅
  - [ ] `Profile()` → Affiche le profil utilisateur ⚠️ Non implémenté (endpoint API n'existe pas)
  - [x] `ConfirmEmail(userId, token)` GET → Confirme l'email via l'API ✅ **AJOUTÉ**
  - [x] `ForgotPassword()` GET → Affiche le formulaire de demande de réinitialisation ✅
  - [x] `ForgotPassword(model)` POST → Demande de réinitialisation via l'API ✅ **AJOUTÉ**
  - [x] `ResetPassword(email, token)` GET → Affiche le formulaire de réinitialisation ✅
  - [x] `ResetPassword(model)` POST → Réinitialise le mot de passe via l'API ✅
  - [ ] `AccessDenied()` → Page accès refusé ⚠️ Optionnel
- [x] Vérifier que le token est stocké après connexion réussie ✅
- [x] Vérifier la redirection après login/logout ✅

### 3.5 CartController
- [x] Vérifier l'injection de `ICartApiService` ✅
- [x] Vérifier les actions :
  - [x] `Index()` → Affiche le panier ✅
  - [x] `AddToCart(productId, quantity)` POST → Ajoute au panier ✅
  - [x] `UpdateQuantity(itemId, quantity)` POST → Met à jour la quantité ✅
  - [x] `Remove(itemId)` POST → Retire un article ✅
  - [ ] `Clear()` POST → Vide le panier ⚠️ Non implémenté (utilise RemoveCoupon)
  - [x] `Checkout()` GET → Page de validation ✅
- [ ] Vérifier l'affichage du nombre d'articles dans le menu

### 3.6 OrderController
- [x] Vérifier l'injection de `IOrderApiService` ✅
- [x] Vérifier les actions :
  - [x] `Index()` → Liste les commandes ✅
  - [x] `Details(id)` → Affiche les détails d'une commande ✅
  - [x] `Create()` GET/POST → Crée une commande depuis le panier ✅
  - [ ] `MyOrders()` → Liste mes commandes ⚠️ Fusionné avec Index
  - [ ] `UpdateStatus(id, status)` POST → Met à jour le statut (admin) ⚠️ Non implémenté
  - [x] `CancelOrder(id)` POST → Annule une commande ✅

### 3.7 CouponController
- [x] Vérifier l'injection de `ICouponApiService` ✅
- [x] Vérifier les actions :
  - [x] `Index()` → Liste les coupons (admin) ✅
  - [x] `Create()` GET/POST → Créer un coupon ✅
  - [x] `Edit(id)` GET/POST → Modifier un coupon ✅ **AJOUTÉ**
  - [x] `Delete(id)` GET/POST → Supprimer un coupon ✅ **AJOUTÉ**
  - [ ] `Apply(code)` POST → Appliquer un coupon au panier (dans CartController)

---

## 🎨 PHASE 4 : Vues Razor - Vérification et Adaptation

### 4.1 Layout et Vues Partagées
- [ ] Vérifier `Views/Shared/_Layout.cshtml` :
  - [ ] Navigation responsive
  - [ ] Affichage conditionnel (connecté/déconnecté)
  - [ ] Icône panier avec compteur d'articles
  - [ ] Messages TempData (Success/Error)
  - [ ] Menu Admin visible uniquement pour les admins
- [ ] Vérifier `Views/Shared/_LoginPartial.cshtml`
- [ ] Vérifier `Views/Shared/_ValidationScriptsPartial.cshtml`
- [ ] Vérifier `Views/Shared/Error.cshtml`

### 4.2 Vues Home
- [ ] `Index.cshtml` → Page d'accueil avec produits populaires
- [ ] `Privacy.cshtml` → Page de confidentialité
- [ ] `ApiError.cshtml` → Message d'erreur API

### 4.3 Vues Authentification (Views/Auth/)
- [x] `Login.cshtml` → Formulaire de connexion ✅
- [x] `Register.cshtml` → Formulaire d'inscription ✅
- [ ] `Profile.cshtml` → Page profil utilisateur ⚠️ Non implémenté (endpoint API n'existe pas)
- [ ] `ConfirmEmail.cshtml` → Page de confirmation d'email ⚠️ Pas nécessaire (redirection directe)
- [x] `ForgotPassword.cshtml` → Formulaire de demande de réinitialisation ✅ **AJOUTÉ**
- [x] `ResetPassword.cshtml` → Formulaire de réinitialisation de mot de passe ✅
- [ ] `AccessDenied.cshtml` → Page accès refusé ⚠️ Optionnel
- [x] Vérifier que les erreurs de validation s'affichent correctement ✅

### 4.4 Vues Product
- [x] `Index.cshtml` → Liste des produits avec pagination ✅
- [x] `Details.cshtml` → Détails d'un produit + bouton "Ajouter au panier" ✅ **AJOUTÉ**
- [x] `Create.cshtml` → Formulaire de création avec upload d'image ✅
- [x] `Edit.cshtml` → Formulaire de modification ✅
- [x] `Delete.cshtml` → Confirmation de suppression ✅
- [x] Vérifier que les images s'affichent correctement ✅
- [x] Vérifier que les dropdowns de catégories fonctionnent ✅

### 4.5 Vues Category
- [x] `Index.cshtml` → Liste des catégories ✅
- [x] `Create.cshtml` → Formulaire de création ✅
- [x] `Edit.cshtml` → Formulaire de modification ✅
- [x] `Delete.cshtml` → Confirmation de suppression ✅

### 4.6 Vues Cart
- [x] `Index.cshtml` → Affichage du panier ✅
  - [x] Liste des articles ✅
  - [x] Quantités modifiables ✅
  - [x] Prix unitaires et totaux ✅
  - [ ] Bouton "Vider le panier" ⚠️
  - [x] Bouton "Commander" ✅
  - [x] Application de coupon ✅
- [x] `Checkout.cshtml` → Page de validation de commande ✅
- [ ] `_CartSummary.cshtml` → Partial view pour le résumé ⚠️ Optionnel

### 4.7 Vues Order
- [x] `Index.cshtml` → Liste des commandes ✅
- [x] `Details.cshtml` → Détails d'une commande ✅
  - [x] Informations client ✅
  - [x] Liste des articles ✅
  - [x] Statut de la commande ✅
  - [ ] Bouton de changement de statut (admin) ⚠️ Optionnel
- [x] `Confirmation.cshtml` → Confirmation après commande ✅
- [ ] `MyOrders.cshtml` → Historique des commandes utilisateur ⚠️ Fusionné avec Index

### 4.8 Vues Coupon
- [x] `Index.cshtml` → Liste des coupons (admin) ✅
- [x] `Create.cshtml` → Formulaire de création ✅
- [x] `Edit.cshtml` → Formulaire de modification ✅ **AJOUTÉ**
- [x] `Delete.cshtml` → Confirmation de suppression ✅ **AJOUTÉ**

---

## 🔐 PHASE 5 : Authentification et Sécurité

### 5.1 Gestion du Token JWT
- [x] Vérifier que le token est stocké en Session après connexion : ✅
  ```csharp
  HttpContext.Session.SetString("JwtToken", tokenResponse.AccessToken);
  ```
- [x] Vérifier que chaque Service HTTP récupère le token : ✅
  ```csharp
  var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
  ```
- [x] Vérifier que le token est ajouté aux en-têtes HTTP : ✅
  ```csharp
  _httpClient.DefaultRequestHeaders.Authorization = 
      new AuthenticationHeaderValue("Bearer", token);
  ```

### 5.2 Attribut d'autorisation personnalisé
- [x] Vérifier l'existence de `Helpers/AuthorizeApiAttribute.cs` ✅
- [x] Vérifier la logique de vérification du token ✅
- [x] Vérifier la redirection vers Login si non authentifié ✅
- [x] Vérifier la gestion des tokens expirés ✅

### 5.3 Application de l'attribut [AuthorizeApi]
- [x] Vérifier sur les actions du CartController ✅
- [x] Vérifier sur les actions du OrderController ✅
- [x] Vérifier sur les actions admin (Product.Create, Product.Edit, etc.) ✅ (via Controller)
- [x] Vérifier sur Profile dans AuthController ✅ (Optionnel, non implémenté)

### 5.4 Gestion des erreurs 401 Unauthorized
- [x] Vérifier que les erreurs 401 redirigent vers Login ✅
- [x] Afficher un message explicatif : "Session expirée, veuillez vous reconnecter" ✅

---

## ⚠️ PHASE 6 : Gestion des Erreurs

### 6.1 Erreurs réseau (API indisponible)
- [x] Dans chaque Controller, capturer `HttpRequestException` ✅ (via Middleware ou Try/Catch)
- [x] Afficher un message utilisateur compréhensible ✅
- [x] Exemple :
  ```csharp
  catch (HttpRequestException)
  {
      TempData["Error"] = "Impossible de se connecter au serveur.";
      return View(new List<ProductViewModel>());
  }
  ```

### 6.2 Erreurs de validation
- [x] Vérifier que les erreurs de l'API sont affichées dans les vues ✅
- [x] Utiliser `ModelState.AddModelError()` ✅
- [x] Vérifier `@Html.ValidationSummary()` dans les vues ✅

### 6.3 Page d'erreur API
- [x] Vérifier `Views/Home/ApiError.cshtml` ✅ **CRÉÉ**
- [x] Afficher un message clair : "Le serveur API est indisponible" ✅
- [x] Ajouter un bouton "Réessayer" ✅

---

## 📦 PHASE 7 : Modèles et DTOs

### 7.1 ApiResponses
- [ ] Vérifier `Models/ApiResponses/ApiResponse.cs`
- [ ] Vérifier la structure :
  ```csharp
  public class ApiResponse<T>
  {
      public bool Success { get; set; }
      public string Message { get; set; }
      public T Data { get; set; }
      public List<string> Errors { get; set; }
  }
  ```

### 7.2 Configuration
- [ ] Vérifier `Models/Configuration/ApiSettings.cs`
- [ ] Vérifier `Models/Configuration/JwtSettings.cs`

### 7.3 ViewModels
- [x] Vérifier tous les ViewModels dans `Models/ViewModels/` :
  - [x] Products (ProductViewModel, CreateProductViewModel, EditProductViewModel) ✅
  - [x] Categories (CategoryViewModel, CreateCategoryViewModel, etc.) ✅
  - [x] Auth (LoginViewModel, RegisterViewModel, UserInfoViewModel) ✅
  - [x] Cart (CartViewModel, CartItemViewModel) ✅
  - [x] Orders (OrderViewModel, CreateOrderViewModel, OrderItemViewModel) ✅
  - [x] Coupons (CouponViewModel, CreateCouponViewModel) ✅

### 7.4 ApiDtos (si utilisés)
- [ ] Vérifier que les DTOs correspondent aux réponses de l'API
- [ ] Vérifier les mappings entre DTOs et ViewModels si nécessaire

---

## ⚙️ PHASE 8 : Configuration et Paramètres

### 8.1 appsettings.json
- [x] Vérifier la configuration de l'API :
  ```json
  {
    "ApiSettings": {
      "BaseUrl": "http://localhost:5225/api/",
      "Timeout": 30
    },
    "JwtSettings": {
      "CookieName": "JwtToken",
      "RefreshCookieName": "RefreshToken"
    }
  }
  ```
  ✅ **Configuration existante vérifiée**
  
  ⚠️ **Note** : Le port de l'API est 5225. Doit correspondre au port sur lequel l'API est lancée.

### 8.2 Program.cs
- [x] Vérifier la configuration `HttpClient` :
  ```csharp
  builder.Services.AddHttpClient("ApiClient", client =>
  {
      client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
      client.Timeout = TimeSpan.FromSeconds(30);
  });
  ```
  ✅ **Configuré correctement**
  
- [x] Vérifier l'injection des services :
  - [x] `IProductApiService` ✅
  - [x] `ICategoryApiService` ✅
  - [x] `IAuthApiService` ✅
  - [x] `ICartApiService` ✅
  - [x] `IOrderApiService` ✅
  - [x] `ICouponApiService` ✅
  
- [x] Vérifier la configuration Session :
  ```csharp
  builder.Services.AddSession(options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(30);
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
  });
  ```
  ✅ **Configuré correctement**
  
- [x] Vérifier `AddHttpContextAccessor()` pour accéder à la Session ✅
- [x] Vérifier l'ordre des middlewares : `app.UseSession()` avant `app.UseRouting()` ✅
- [x] Middleware `ApiExceptionMiddleware` présent ✅

### 8.3 launchSettings.json
- [x] Vérifier que le port est différent de l'API (ex: 5002 pour le client) ✅
- [x] Exemple :
  ```json
  "applicationUrl": "https://localhost:7064;http://localhost:5295"
  ```

---

## 🎨 PHASE 9 : Assets et Ressources

### 9.1 CSS
- [x] Vérifier que `wwwroot/css/` contient tous les styles ✅
- [x] Vérifier que les styles correspondent au projet MVC original ✅
- [x] Vérifier `site.css` pour les styles personnalisés ✅
- [ ] Vérifier Bootstrap ou autre framework CSS utilisé ✅

### 9.2 JavaScript
- [x] Vérifier que `wwwroot/js/` contient tous les scripts ✅
- [x] Vérifier `site.js` pour les fonctionnalités JS personnalisées ✅
- [ ] Vérifier les scripts de validation côté client ✅
- [x] Vérifier les scripts AJAX pour les appels dynamiques (si utilisés) ✅ (`order.js`)

### 9.3 Images
- [x] Vérifier que `wwwroot/images/` contient toutes les images nécessaires ✅
- [x] Vérifier les images de produits (si elles sont stockées localement) ✅
- [ ] Vérifier les icônes et logos ✅

---

## 🧪 PHASE 10 : Tests Manuels

### 10.1 Préparation
- [ ] Démarrer l'API :
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet API\Formation-Ecommerce-11-2025.API"
  dotnet run
  ```
- [ ] Vérifier que l'API démarre sur `https://localhost:5001`
- [ ] Vérifier Swagger : `https://localhost:5001/swagger`

- [ ] Démarrer le Client MVC :
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet CLient MVC\Formation-Ecommerce-Client"
  dotnet run
  ```
- [ ] Vérifier que le client démarre sur `https://localhost:5002`

### 10.2 Tests Authentification
- [ ] Accéder à la page d'accueil
- [ ] Cliquer sur "S'inscrire"
- [ ] Remplir le formulaire d'inscription
- [ ] Vérifier que#### Authentification
- [ ] Inscription d'un nouvel utilisateur
- [ ] Vérifier l'envoi de l'email de confirmation
- [ ] Confirmer l'email via le lien reçu
- [ ] Connexion avec le compte créé et confirmé
- [ ] Vérification que le menu change après connexion
- [ ] Déconnexion
- [ ] Test "Mot de passe oublié" :
  - [ ] Demander une réinitialisation
  - [ ] Vérifier l'envoi de l'email
  - [ ] Réinitialiser le mot de passe via le lien
  - [ ] Se connecter avec le nouveau mot de passefier l'affichage du nom d'utilisateur
- [ ] Accéder au profil utilisateur
- [ ] Se déconnecter

### 10.3 Tests Produits
- [ ] Accéder à la liste des produits
- [ ] Vérifier que tous les produits s'affichent
- [ ] Cliquer sur un produit pour voir les détails
- [ ] Vérifier l'affichage de l'image, prix, description
- [ ] Se connecter en tant qu'admin
- [ ] Créer un nouveau produit :
  - [ ] Remplir tous les champs
  - [ ] Uploader une image
  - [ ] Sélectionner une catégorie
  - [ ] Soumettre
- [ ] Vérifier que le produit apparaît dans la liste
- [ ] Modifier le produit créé
- [ ] Vérifier que les modifications sont sauvegardées
- [ ] Supprimer le produit
- [ ] Vérifier que le produit est bien supprimé

### 10.4 Tests Catégories
- [ ] Accéder à la liste des catégories
- [ ] Créer une nouvelle catégorie
- [ ] Modifier une catégorie existante
- [ ] Vérifier que les produits de cette catégorie s'affichent correctement
- [ ] Supprimer une catégorie (sans produits associés)

### 10.5 Tests Panier
- [ ] Se connecter en tant qu'utilisateur
- [ ] Accéder à un produit
- [ ] Ajouter le produit au panier
- [ ] Vérifier que le compteur du panier s'incrémente
- [ ] Accéder au panier
- [ ] Vérifier l'affichage du produit
- [ ] Modifier la quantité
- [ ] Vérifier que le total se met à jour
- [ ] Ajouter un autre produit
- [ ] Retirer un produit du panier
- [ ] Vider le panier
- [ ] Vérifier que le panier est vide

### 10.6 Tests Commandes
- [ ] Ajouter des produits au panier
- [ ] Accéder au panier
- [ ] Cliquer sur "Commander"
- [ ] Remplir les informations de commande
- [ ] Soumettre la commande
- [ ] Vérifier la page de confirmation
- [ ] Accéder à "Mes commandes"
- [ ] Vérifier que la commande apparaît
- [ ] Cliquer sur la commande pour voir les détails
- [ ] Se connecter en tant qu'admin
- [ ] Accéder à la liste de toutes les commandes
- [ ] Changer le statut d'une commande
- [ ] Vérifier que le statut est mis à jour

### 10.7 Tests Coupons
- [ ] Se connecter en tant qu'admin
- [ ] Accéder à la liste des coupons
- [ ] Créer un nouveau coupon :
  - [ ] Code promo
  - [ ] Pourcentage ou montant fixe
  - [ ] Date d'expiration
- [ ] Se connecter en tant qu'utilisateur
- [ ] Ajouter des produits au panier
- [ ] Appliquer le code promo
- [ ] Vérifier que la réduction s'applique
- [ ] Vérifier le calcul du total
- [ ] Passer la commande avec le coupon

### 10.8 Tests Gestion des Erreurs
- [ ] Arrêter l'API
- [ ] Tenter d'accéder à la liste des produits
- [ ] Vérifier l'affichage du message d'erreur
- [ ] Redémarrer l'API
- [ ] Rafraîchir la page
- [ ] Vérifier que les produits s'affichent à nouveau
- [ ] Tester un token expiré (simuler en supprimant le token de la session)
- [ ] Vérifier la redirection vers Login

---

## 🔍 PHASE 11 : Comparaison avec le Projet Original

### 11.1 Fonctionnalités identiques
- [ ] Lister toutes les fonctionnalités du projet MVC original :
  - [ ] Authentification (Login/Register/Logout)
  - [ ] Gestion des produits (CRUD)
  - [ ] Gestion des catégories (CRUD)
  - [ ] Panier d'achat
  - [ ] Commandes
  - [ ] Coupons de réduction
  - [ ] Rôles (Admin/User)
  - [ ] Profil utilisateur
  - [ ] ... (autres fonctionnalités)

- [ ] Vérifier que chaque fonctionnalité est présente dans le Client MVC

### 11.2 UX identique
- [ ] Comparer visuellement les pages :
  - [ ] Page d'accueil
  - [ ] Liste des produits
  - [ ] Détails produit
  - [ ] Panier
  - [ ] Formulaire de commande
  - [ ] Toutes les autres pages

- [ ] Vérifier que l'utilisateur ne voit aucune différence
- [ ] Vérifier la navigation
- [ ] Vérifier les messages d'erreur et de succès

### 11.3 Performances
- [ ] Comparer les temps de chargement
- [ ] Identifier si des latences réseau sont perceptibles
- [ ] Optimiser si nécessaire (cache, compression, etc.)

---

## 📝 PHASE 12 : Documentation et Finalisation

### 12.1 Documentation du code
- [ ] Ajouter des commentaires XML sur les méthodes publiques
- [ ] Documenter les Services HTTP
- [ ] Documenter les Controllers

### 12.2 README.md
- [ ] Créer un `README.md` pour le projet Client MVC
- [ ] Expliquer l'architecture
- [ ] Expliquer comment démarrer le projet
- [ ] Lister les prérequis (API en cours d'exécution)
- [ ] Fournir des exemples d'utilisation

### 12.3 Script de démarrage
- [ ] Vérifier/créer `start-dev.bat` :
  ```batch
  @echo off
  echo Démarrage de l'API...
  start cmd /k "cd /d C:\...\Projet API\Formation-Ecommerce-11-2025.API && dotnet run"
  
  echo Attente de 5 secondes...
  timeout /t 5 /nobreak
  
  echo Démarrage du Client MVC...
  start cmd /k "cd /d C:\...\Projet CLient MVC\Formation-Ecommerce-Client && dotnet run"
  
  echo Environnement prêt !
  echo API : https://localhost:5001
  echo Client : https://localhost:5002
  ```

### 12.4 Guide de formation
- [ ] Créer un guide pédagogique expliquant :
  - [ ] L'évolution de l'architecture (MVC → API + Client)
  - [ ] Les avantages de cette architecture
  - [ ] Les concepts clés (Services HTTP, JWT, API REST)
  - [ ] Les exercices pratiques pour les étudiants

---

## ✅ PHASE 13 : Validation Finale

### 13.1 Checklist de validation
- [ ] Tous les services HTTP sont implémentés et testés
- [ ] Tous les controllers sont implémentés et testés
- [ ] Toutes les vues sont créées et fonctionnelles
- [ ] L'authentification JWT fonctionne correctement
- [ ] La gestion des erreurs est robuste
- [ ] L'UX est identique au projet original
- [ ] Aucune erreur de compilation
- [ ] Aucune erreur au runtime
- [ ] Les tests manuels passent tous

### 13.2 Préparation pour la formation
- [ ] Préparer une démo complète
- [ ] Préparer des slides expliquant l'architecture
- [ ] Préparer des exercices pour les étudiants
- [ ] Préparer des solutions aux exercices
- [ ] Tester tout le processus de A à Z

---

## 🎯 Objectifs Pédagogiques

À la fin de cette formation, les étudiants devront comprendre :

1. **Architecture Client-Serveur** :
   - Séparation des responsabilités
   - Communication via API REST
   - Avantages et inconvénients

2. **Consommation d'API** :
   - Utilisation de HttpClient
   - Gestion des en-têtes HTTP
   - Sérialisation/Désérialisation JSON

3. **Authentification JWT** :
   - Stockage du token côté client
   - Envoi du token avec chaque requête
   - Gestion de l'expiration

4. **Gestion des erreurs** :
   - Erreurs réseau
   - Erreurs de validation
   - Expérience utilisateur en cas d'erreur

5. **Différences architecturales** :
   - MVC monolithique vs Client-Serveur
   - Réutilisabilité du code
   - Scalabilité

---

## 📊 Tableau de Comparaison

| Aspect | MVC Original | Client MVC + API |
|--------|-------------|------------------|
| **Accès aux données** | Direct (EF Core) | Via HTTP (API REST) |
| **Authentification** | ASP.NET Identity (Cookie) | JWT Token (Session) |
| **Couches** | Toutes dans un projet | Séparées (API / Client) |
| **Réutilisabilité** | Web uniquement | API réutilisable (Mobile, SPA, etc.) |
| **Scalabilité** | Limitée | Indépendante (API / Client) |
| **Latence** | Minimale | Légère latence réseau |
| **Complexité** | Plus simple | Plus modulaire |

---

## 🎓 Ressources Complémentaires

- [ASP.NET Core HttpClient](https://docs.microsoft.com/aspnet/core/fundamentals/http-requests)
- [JWT Introduction](https://jwt.io/introduction)
- [REST API Best Practices](https://restfulapi.net/)
- [ASP.NET Core MVC](https://docs.microsoft.com/aspnet/core/mvc/overview)

---

**🎉 Bon courage pour la finalisation du projet !**
