# 🖥️ Guide de Création : Client MVC Razor - Consommateur d'API

## 📋 Vue d'ensemble

Ce document détaille les étapes pour créer un projet **ASP.NET Core MVC Razor** qui consomme l'API E-Commerce créée dans la formation.

### Architecture Cible 🎯
```
┌─────────────────────────────────────────┐
│         CLIENT MVC RAZOR                │
│  ┌───────────────────────────────────┐  │
│  │  Controllers → Appellent Services│  │
│  │  Views (CSHTML) → Interface UI   │  │
│  │  Services HTTP → Consomment API  │  │
│  │  ViewModels → Données pour Views │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
                    │
                    │ HTTP/REST (JSON)
                    ▼
┌─────────────────────────────────────────┐
│         SERVEUR API REST                │
│   Formation-Ecommerce-11-2025.API       │
│          (Port: 5001)                   │
└─────────────────────────────────────────┘
```

---

## 📝 CHECKLIST DÉTAILLÉE

### PHASE 1 : Initialisation du Projet 🚀

- [x] **1.1 Créer le projet MVC**
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet CLient MVC"
  dotnet new mvc -n Formation-Ecommerce-Client
  ```

- [x] **1.2 Ouvrir le projet dans VS/VS Code**
  ```bash
  cd Formation-Ecommerce-Client
  code .
  ```

- [x] **1.3 Installer les packages NuGet nécessaires**
  ```bash
  # Client HTTP typé avec Refit (optionnel mais recommandé)
  dotnet add package Refit
  dotnet add package Refit.HttpClientFactory
  
  # OU utiliser HttpClient natif avec :
  dotnet add package Microsoft.Extensions.Http
  
  # Pour la gestion des tokens JWT côté client
  dotnet add package System.IdentityModel.Tokens.Jwt
  ```

---

### PHASE 2 : Structure des Dossiers 📁

- [x] **2.1 Créer la structure de dossiers**
  ```
  Formation-Ecommerce-Client/
  ├── Controllers/
  │   ├── HomeController.cs
  │   ├── ProductController.cs
  │   ├── CategoryController.cs
  │   ├── AuthController.cs
  │   ├── CartController.cs
  │   └── OrderController.cs
  ├── Models/
  │   ├── ViewModels/
  │   │   ├── Products/
  │   │   ├── Categories/
  │   │   ├── Auth/
  │   │   ├── Cart/
  │   │   └── Orders/
  │   └── ApiResponses/
  │       └── ApiResponse.cs
  ├── Services/
  │   ├── Interfaces/
  │   │   ├── IProductApiService.cs
  │   │   ├── ICategoryApiService.cs
  │   │   ├── IAuthApiService.cs
  │   │   ├── ICartApiService.cs
  │   │   └── IOrderApiService.cs
  │   └── Implementations/
  │       ├── ProductApiService.cs
  │       ├── CategoryApiService.cs
  │       ├── AuthApiService.cs
  │       ├── CartApiService.cs
  │       └── OrderApiService.cs
  ├── Helpers/
  │   ├── TokenHandler.cs
  │   └── ApiHttpClientHandler.cs
  ├── Views/
  │   ├── (structure similaire au projet MVC original)
  └── wwwroot/
      ├── css/
      ├── js/
      └── images/
  ```

---

### PHASE 3 : Configuration de l'Application ⚙️

- [x] **3.1 Configurer appsettings.json**
  ```json
  {
    "ApiSettings": {
      "BaseUrl": "https://localhost:5001/api",
      "Timeout": 30
    },
    "JwtSettings": {
      "CookieName": "JwtToken",
      "RefreshCookieName": "RefreshToken"
    },
    "Logging": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
  ```

- [x] **3.2 Créer la classe de configuration**
  ```csharp
  // Models/Configuration/ApiSettings.cs
  public class ApiSettings
  {
      public string BaseUrl { get; set; }
      public int Timeout { get; set; }
  }
  ```

- [x] **3.3 Configurer Program.cs**
  ```csharp
  var builder = WebApplication.CreateBuilder(args);
  
  // Configuration des settings
  builder.Services.Configure<ApiSettings>(
      builder.Configuration.GetSection("ApiSettings"));
  
  // Configuration HttpClient
  builder.Services.AddHttpClient("ApiClient", client =>
  {
      client.BaseAddress = new Uri(
          builder.Configuration["ApiSettings:BaseUrl"]);
      client.Timeout = TimeSpan.FromSeconds(30);
  });
  
  // Injection des services API
  builder.Services.AddScoped<IProductApiService, ProductApiService>();
  builder.Services.AddScoped<ICategoryApiService, CategoryApiService>();
  builder.Services.AddScoped<IAuthApiService, AuthApiService>();
  builder.Services.AddScoped<ICartApiService, CartApiService>();
  builder.Services.AddScoped<IOrderApiService, OrderApiService>();
  
  // Session pour stocker le token
  builder.Services.AddSession(options =>
  {
      options.IdleTimeout = TimeSpan.FromMinutes(30);
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
  });
  
  builder.Services.AddControllersWithViews();
  
  var app = builder.Build();
  
  app.UseSession();
  app.UseStaticFiles();
  app.UseRouting();
  
  app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");
  
  app.Run();
  ```

---

### PHASE 4 : Créer les Modèles (ViewModels & DTOs) 📦

- [x] **4.1 Créer le modèle de réponse API générique**
  ```csharp
  // Models/ApiResponses/ApiResponse.cs
  public class ApiResponse<T>
  {
      public bool Success { get; set; }
      public string Message { get; set; }
      public T Data { get; set; }
      public List<string> Errors { get; set; }
  }
  ```

- [x] **4.2 Créer les ViewModels Product**
  ```csharp
  // Models/ViewModels/Products/ProductViewModel.cs
  public class ProductViewModel
  {
      public Guid Id { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public decimal Price { get; set; }
      public string ImageUrl { get; set; }
      public string CategoryName { get; set; }
      public Guid CategoryId { get; set; }
  }
  
  // Models/ViewModels/Products/CreateProductViewModel.cs
  public class CreateProductViewModel
  {
      [Required(ErrorMessage = "Le nom est requis")]
      public string Name { get; set; }
      
      public string Description { get; set; }
      
      [Required]
      [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
      public decimal Price { get; set; }
      
      public IFormFile ImageFile { get; set; }
      
      [Required(ErrorMessage = "La catégorie est requise")]
      public Guid CategoryId { get; set; }
  }
  ```

- [x] **4.3 Créer les ViewModels Auth**
  ```csharp
  // Models/ViewModels/Auth/LoginViewModel.cs
  public class LoginViewModel
  {
      [Required(ErrorMessage = "L'email est requis")]
      [EmailAddress]
      public string Email { get; set; }
      
      [Required(ErrorMessage = "Le mot de passe est requis")]
      [DataType(DataType.Password)]
      public string Password { get; set; }
      
      public bool RememberMe { get; set; }
  }
  
  // Models/ViewModels/Auth/RegisterViewModel.cs
  public class RegisterViewModel
  {
      [Required]
      public string FirstName { get; set; }
      
      [Required]
      public string LastName { get; set; }
      
      [Required]
      [EmailAddress]
      public string Email { get; set; }
      
      [Required]
      [DataType(DataType.Password)]
      [MinLength(6)]
      public string Password { get; set; }
      
      [Required]
      [Compare("Password")]
      public string ConfirmPassword { get; set; }
  }
  
  // Models/ViewModels/Auth/TokenResponse.cs
  public class TokenResponse
  {
      public string AccessToken { get; set; }
      public string RefreshToken { get; set; }
      public DateTime ExpiresAt { get; set; }
      public UserInfo User { get; set; }
  }
  ```

- [x] **4.4 Créer les ViewModels Category, Cart, Order**
  - Répliquer le pattern pour chaque entité

---

### PHASE 5 : Créer les Services HTTP 🌐

- [x] **5.1 Créer l'interface de base**
  ```csharp
  // Services/Interfaces/IApiServiceBase.cs
  public interface IApiServiceBase<TDto, TCreateDto, TUpdateDto>
  {
      Task<IEnumerable<TDto>> GetAllAsync();
      Task<TDto> GetByIdAsync(Guid id);
      Task<TDto> CreateAsync(TCreateDto dto);
      Task UpdateAsync(Guid id, TUpdateDto dto);
      Task DeleteAsync(Guid id);
  }
  ```

- [x] **5.2 Créer le service API pour les produits**
  ```csharp
  // Services/Implementations/ProductApiService.cs
  public class ProductApiService : IProductApiService
  {
      private readonly HttpClient _httpClient;
      private readonly IHttpContextAccessor _httpContextAccessor;
      
      public ProductApiService(
          IHttpClientFactory httpClientFactory,
          IHttpContextAccessor httpContextAccessor)
      {
          _httpClient = httpClientFactory.CreateClient("ApiClient");
          _httpContextAccessor = httpContextAccessor;
          
          // Ajouter le token JWT si disponible
          var token = _httpContextAccessor.HttpContext?.Session
              .GetString("JwtToken");
          if (!string.IsNullOrEmpty(token))
          {
              _httpClient.DefaultRequestHeaders.Authorization = 
                  new AuthenticationHeaderValue("Bearer", token);
          }
      }
      
      public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
      {
          var response = await _httpClient.GetAsync("/products");
          response.EnsureSuccessStatusCode();
          
          var content = await response.Content
              .ReadFromJsonAsync<ApiResponse<IEnumerable<ProductViewModel>>>();
          return content.Data;
      }
      
      public async Task<ProductViewModel> GetByIdAsync(Guid id)
      {
          var response = await _httpClient.GetAsync($"/products/{id}");
          response.EnsureSuccessStatusCode();
          
          var content = await response.Content
              .ReadFromJsonAsync<ApiResponse<ProductViewModel>>();
          return content.Data;
      }
      
      public async Task<ProductViewModel> CreateAsync(
          CreateProductViewModel model)
      {
          // Pour les fichiers, utiliser MultipartFormDataContent
          using var content = new MultipartFormDataContent();
          content.Add(new StringContent(model.Name), "Name");
          content.Add(new StringContent(model.Price.ToString()), "Price");
          content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
          
          if (model.ImageFile != null)
          {
              var streamContent = new StreamContent(model.ImageFile.OpenReadStream());
              content.Add(streamContent, "ImageFile", model.ImageFile.FileName);
          }
          
          var response = await _httpClient.PostAsync("/products", content);
          response.EnsureSuccessStatusCode();
          
          var result = await response.Content
              .ReadFromJsonAsync<ApiResponse<ProductViewModel>>();
          return result.Data;
      }
      
      public async Task UpdateAsync(Guid id, UpdateProductViewModel model)
      {
          var response = await _httpClient.PutAsJsonAsync($"/products/{id}", model);
          response.EnsureSuccessStatusCode();
      }
      
      public async Task DeleteAsync(Guid id)
      {
          var response = await _httpClient.DeleteAsync($"/products/{id}");
          response.EnsureSuccessStatusCode();
      }
  }
  ```

- [x] **5.3 Créer le service Auth**
  ```csharp
  // Services/Implementations/AuthApiService.cs
  public class AuthApiService : IAuthApiService
  {
      private readonly HttpClient _httpClient;
      private readonly IHttpContextAccessor _httpContextAccessor;
      
      public AuthApiService(
          IHttpClientFactory httpClientFactory,
          IHttpContextAccessor httpContextAccessor)
      {
          _httpClient = httpClientFactory.CreateClient("ApiClient");
          _httpContextAccessor = httpContextAccessor;
      }
      
      public async Task<TokenResponse> LoginAsync(LoginViewModel model)
      {
          var response = await _httpClient.PostAsJsonAsync("/auth/login", model);
          
          if (!response.IsSuccessStatusCode)
          {
              throw new UnauthorizedAccessException("Identifiants invalides");
          }
          
          var result = await response.Content
              .ReadFromJsonAsync<ApiResponse<TokenResponse>>();
          
          // Stocker le token en session
          _httpContextAccessor.HttpContext?.Session
              .SetString("JwtToken", result.Data.AccessToken);
          
          return result.Data;
      }
      
      public async Task<bool> RegisterAsync(RegisterViewModel model)
      {
          var response = await _httpClient.PostAsJsonAsync("/auth/register", model);
          return response.IsSuccessStatusCode;
      }
      
      public Task LogoutAsync()
      {
          _httpContextAccessor.HttpContext?.Session.Remove("JwtToken");
          return Task.CompletedTask;
      }
  }
  ```

- [x] **5.4 Créer les services Category, Cart, Order**
  - Même pattern que ProductApiService

---

### PHASE 6 : Créer les Controllers 🎮


- [x] **6.0 Audit des Contrôleurs existants**
  - Analyser le projet source `Formation-Ecommerce-11-2025`
  - Comparer pour identifier les actions manquantes (Coupons, etc.)
  - S'assurer que le Client couvre toutes les fonctionnalités nécessaires

- [x] **6.1 Créer le ProductController**

  ```csharp
  // Controllers/ProductController.cs
  public class ProductController : Controller
  {
      private readonly IProductApiService _productService;
      private readonly ICategoryApiService _categoryService;
      
      public ProductController(
          IProductApiService productService,
          ICategoryApiService categoryService)
      {
          _productService = productService;
          _categoryService = categoryService;
      }
      
      public async Task<IActionResult> Index()
      {
          try
          {
              var products = await _productService.GetAllAsync();
              return View(products);
          }
          catch (HttpRequestException ex)
          {
              TempData["Error"] = "Erreur de connexion à l'API";
              return View(new List<ProductViewModel>());
          }
      }
      
      public async Task<IActionResult> Create()
      {
          var categories = await _categoryService.GetAllAsync();
          ViewBag.Categories = new SelectList(categories, "Id", "Name");
          return View();
      }
      
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(CreateProductViewModel model)
      {
          if (!ModelState.IsValid)
          {
              var categories = await _categoryService.GetAllAsync();
              ViewBag.Categories = new SelectList(categories, "Id", "Name");
              return View(model);
          }
          
          try
          {
              await _productService.CreateAsync(model);
              TempData["Success"] = "Produit créé avec succès!";
              return RedirectToAction(nameof(Index));
          }
          catch (Exception ex)
          {
              TempData["Error"] = $"Erreur: {ex.Message}";
              return View(model);
          }
      }
      
      // Edit, Delete, Details... même pattern
  }
  ```

- [x] **6.2 Créer le AuthController**
  ```csharp
  // Controllers/AuthController.cs
  public class AuthController : Controller
  {
      private readonly IAuthApiService _authService;
      
      public AuthController(IAuthApiService authService)
      {
          _authService = authService;
      }
      
      public IActionResult Login(string returnUrl = null)
      {
          ViewData["ReturnUrl"] = returnUrl;
          return View();
      }
      
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(
          LoginViewModel model, string returnUrl = null)
      {
          if (!ModelState.IsValid)
              return View(model);
          
          try
          {
              var tokenResponse = await _authService.LoginAsync(model);
              TempData["Success"] = "Connexion réussie!";
              
              return LocalRedirect(returnUrl ?? "/");
          }
          catch (UnauthorizedAccessException)
          {
              ModelState.AddModelError("", "Email ou mot de passe incorrect");
              return View(model);
          }
      }
      
      [HttpPost]
      public async Task<IActionResult> Logout()
      {
          await _authService.LogoutAsync();
          return RedirectToAction("Index", "Home");
      }
  }
  ```

- [x] **6.3 Créer les autres controllers**
  - CategoryController
  - CartController
  - OrderController
  - HomeController

---

### PHASE 7 : Créer les Vues Razor 🎨

- [x] **7.1 Configurer le Layout principal (_Layout.cshtml)**
  - Navigation responsive
  - Affichage conditionnel (connecté/déconnecté)
  - Messages TempData (Success/Error)

- [x] **7.2 Créer les vues Product**
  ```
  Views/Product/
  ├── Index.cshtml      → Liste des produits
  ├── Create.cshtml     → Formulaire création
  ├── Edit.cshtml       → Formulaire modification
  ├── Delete.cshtml     → Confirmation suppression
  └── Details.cshtml    → Détails produit
  ```

- [x] **7.3 Créer les vues Auth**
  ```
  Views/Auth/
  ├── Login.cshtml      → Formulaire connexion
  ├── Register.cshtml   → Formulaire inscription
  └── Profile.cshtml    → Profil utilisateur
  ```

- [x] **7.4 Créer les vues Cart et Order**
  ```
  Views/Cart/
  ├── Index.cshtml      → Panier
  └── Checkout.cshtml   → Validation commande
  
  Views/Order/
  ├── Index.cshtml      → Historique commandes
  └── Details.cshtml    → Détails commande
  ```

- [x] **7.5 Copier et adapter le CSS/JS du projet MVC original**
  - Copier wwwroot/css
  - Copier wwwroot/js
  - Adapter les chemins si nécessaire

---

### PHASE 8 : Gestion des Erreurs et Sécurité 🔐

- [x] **8.1 Créer un ActionFilter pour la gestion des tokens expirés**
  ```csharp
  // Helpers/AuthorizeApiAttribute.cs
  public class AuthorizeApiAttribute : ActionFilterAttribute
  {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
          var token = context.HttpContext.Session.GetString("JwtToken");
          
          if (string.IsNullOrEmpty(token))
          {
              context.Result = new RedirectToActionResult(
                  "Login", "Auth", 
                  new { returnUrl = context.HttpContext.Request.Path });
              return;
          }
          
          // Vérifier si le token est expiré
          var handler = new JwtSecurityTokenHandler();
          var jwtToken = handler.ReadJwtToken(token);
          
          if (jwtToken.ValidTo < DateTime.UtcNow)
          {
              context.HttpContext.Session.Remove("JwtToken");
              context.Result = new RedirectToActionResult(
                  "Login", "Auth", null);
              return;
          }
          
          base.OnActionExecuting(context);
      }
  }
  ```

- [x] **8.2 Créer un middleware de gestion des erreurs API**
  ```csharp
  // Middleware/ApiExceptionMiddleware.cs
  public class ApiExceptionMiddleware
  {
      private readonly RequestDelegate _next;
      
      public async Task InvokeAsync(HttpContext context)
      {
          try
          {
              await _next(context);
          }
          catch (HttpRequestException ex)
          {
              // Rediriger vers une page d'erreur
              context.Response.Redirect("/Home/ApiError");
          }
      }
  }
  ```

- [x] **8.3 Créer la vue d'erreur API**
  ```html
  <!-- Views/Home/ApiError.cshtml -->
  @{
      ViewData["Title"] = "Erreur de connexion";
  }
  
  <div class="alert alert-danger">
      <h4>Impossible de se connecter à l'API</h4>
      <p>Veuillez vérifier que le serveur API est en cours d'exécution.</p>
      <a href="/" class="btn btn-primary">Réessayer</a>
  </div>
  ```

---

### PHASE 9 : Tests et Validation 🧪

- [x] **9.1 Démarrer l'API (dans un terminal séparé)**
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet full stack MVC\Formation-Ecommerce-11-2025\Formation-Ecommerce-11-2025.API"
  dotnet run
  ```

- [x] **9.2 Démarrer le Client MVC**
  ```bash
  cd "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet CLient MVC\Formation-Ecommerce-Client"
  dotnet run
  ```

- [ ] **9.3 Tester les fonctionnalités**
  - [ ] Inscription utilisateur
  - [ ] Connexion/Déconnexion
  - [ ] Liste des produits
  - [ ] Création de produit
  - [ ] Modification de produit
  - [ ] Suppression de produit
  - [ ] Panier d'achat
  - [ ] Passage de commande

---

### PHASE 10 : Configuration pour le Développement 🔧

- [x] **10.1 Configurer launchSettings.json**
  ```json
  {
    "profiles": {
      "Formation-Ecommerce-Client": {
        "commandName": "Project",
        "dotnetRunMessages": true,
        "launchBrowser": true,
        "applicationUrl": "https://localhost:5002;http://localhost:5003",
        "environmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Development"
        }
      }
    }
  }
  ```

- [x] **10.2 Créer un script de démarrage (optionnel)**
  ```batch
  @echo off
  REM start-dev.bat - Démarre API + Client
  start cmd /k "cd /d C:\...\Formation-Ecommerce-11-2025.API && dotnet run"
  timeout /t 5
  start cmd /k "cd /d C:\...\Formation-Ecommerce-Client && dotnet run"
  ```

---

## 📁 Structure Finale du Projet Client

```
Formation-Ecommerce-Client/
├── Controllers/
│   ├── HomeController.cs
│   ├── AuthController.cs
│   ├── ProductController.cs
│   ├── CategoryController.cs
│   ├── CartController.cs
│   └── OrderController.cs
├── Models/
│   ├── Configuration/
│   │   └── ApiSettings.cs
│   ├── ViewModels/
│   │   ├── Products/
│   │   ├── Categories/
│   │   ├── Auth/
│   │   ├── Cart/
│   │   └── Orders/
│   └── ApiResponses/
│       └── ApiResponse.cs
├── Services/
│   ├── Interfaces/
│   │   ├── IProductApiService.cs
│   │   ├── ICategoryApiService.cs
│   │   ├── IAuthApiService.cs
│   │   └── ...
│   └── Implementations/
│       ├── ProductApiService.cs
│       ├── CategoryApiService.cs
│       ├── AuthApiService.cs
│       └── ...
├── Helpers/
│   ├── AuthorizeApiAttribute.cs
│   └── TokenHelper.cs
├── Middleware/
│   └── ApiExceptionMiddleware.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Home/
│   ├── Auth/
│   ├── Product/
│   ├── Category/
│   ├── Cart/
│   └── Order/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
├── Program.cs
├── appsettings.json
└── Formation-Ecommerce-Client.csproj
```

---

## ⏱️ Estimation du Temps

| Phase | Durée estimée |
|-------|---------------|
| Phase 1-2 : Initialisation | 30 min |
| Phase 3 : Configuration | 1 heure |
| Phase 4 : Modèles | 1-2 heures |
| Phase 5 : Services HTTP | 2-3 heures |
| Phase 6 : Controllers | 2 heures |
| Phase 7 : Vues Razor | 3-4 heures |
| Phase 8 : Sécurité | 1 heure |
| Phase 9-10 : Tests | 1 heure |
| **TOTAL** | **12-15 heures** |

---

## 💡 Conseils pour la Formation

1. **Comparer avec le MVC original** : Montrer comment le flux change (Controller → Service HTTP → API → DB vs Controller → Service → Repository → DB)

2. **Démontrer les avantages** :
   - API réutilisable pour d'autres clients (mobile, SPA)
   - Séparation claire des responsabilités
   - Scalabilité indépendante

3. **Points d'attention** :
   - Gestion des tokens JWT
   - Timeout des requêtes HTTP
   - Gestion des erreurs réseau

4. **Exercice pratique** : Faire créer un controller et son service aux étudiants

---

## 🔗 Ressources

- [HttpClient in ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/http-requests)
- [IHttpClientFactory](https://docs.microsoft.com/dotnet/core/extensions/httpclient-factory)
- [Refit - REST library](https://github.com/reactiveui/refit)
