# 🚀 Guide de Transformation : MVC vers API + Client Angular

## 📋 Vue d'ensemble

Ce document détaille les étapes pour transformer le projet **Formation-Ecommerce-11-2025** d'une architecture MVC monolithique vers une architecture **Client/Serveur** avec :
- **Backend** : ASP.NET Core Web API (Clean Architecture)
- **Frontend** : Application Client séparée (Angular/React/Blazor)

---

## ✅ Validation de l'approche

> [!IMPORTANT]
> **OUI, votre approche est correcte !** Grâce à la Clean Architecture déjà en place, vous pouvez simplement remplacer la couche de présentation MVC par une couche API REST.

### Architecture Actuelle ✨
```
┌─────────────────────────────────────────────────────────────┐
│                    COUCHE PRÉSENTATION                       │
│  Formation-Ecommerce-11-2025 (MVC)                          │
│  ├── Controllers/     → Retournent des Views                │
│  ├── Views/           → Razor Views (CSHTML)                │
│  ├── Models/          → ViewModels spécifiques MVC          │
│  └── Mapping/         → AutoMapper Profiles (DTO ↔ VM)      │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    COUCHE APPLICATION                        │
│  Formation-Ecommerce-11-2025.Application                    │
│  ├── Products/Services/    → Logique métier                 │
│  ├── Categories/Services/  → Logique métier                 │
│  ├── Orders/Services/      → Logique métier                 │
│  ├── */Dtos/               → Data Transfer Objects          │
│  └── */Interfaces/         → Contrats de services           │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    COUCHE INFRASTRUCTURE                     │
│  Formation-Ecommerce-11-2025.Infrastructure                 │
│  ├── Persistence/     → DbContext, Repositories             │
│  ├── External/        → Services externes (Email, etc.)     │
│  └── Migrations/      → EF Core Migrations                  │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                    COUCHE DOMAINE                            │
│  Formation-Ecommerce-11-2025.Core                           │
│  ├── Entities/        → Entités métier                       │
│  ├── Interfaces/      → Contrats (Repositories, etc.)       │
│  └── Common/          → Classes partagées                    │
└─────────────────────────────────────────────────────────────┘
```

### Architecture Cible 🎯
```
┌───────────────────────┐     HTTP/REST     ┌────────────────────────┐
│   CLIENT APPLICATION  │◄─────────────────►│   API REST (Serveur)   │
│  (Angular/React/etc.) │     JSON          │  ASP.NET Core Web API  │
└───────────────────────┘                   └────────────────────────┘
                                                      │
                              ┌────────────────────────┘
                              ▼
                    [Application Layer]
                    [Infrastructure Layer]
                    [Core/Domain Layer]
```

---

## 📝 CHECKLIST DÉTAILLÉE DE TRANSFORMATION

### PHASE 1 : Préparation et Configuration ⚙️

- [ ] **1.1 Sauvegarder le projet actuel**
  - Créer une branche Git `feature/api-transformation`
  - Commit de l'état actuel

- [ ] **1.2 Créer le nouveau projet API**
  ```bash
  dotnet new webapi -n Formation-Ecommerce-11-2025.API -o Formation-Ecommerce-11-2025.API
  ```

- [ ] **1.3 Ajouter le projet API à la solution**
  ```bash
  dotnet sln add Formation-Ecommerce-11-2025.API/Formation-Ecommerce-11-2025.API.csproj
  ```

- [ ] **1.4 Configurer les références de projet**
  - Référencer `Formation-Ecommerce-11-2025.Application`
  - Référencer `Formation-Ecommerce-11-2025.Infrastructure`

---

### PHASE 2 : Configuration du Projet API 🔧

- [ ] **2.1 Configurer Program.cs pour l'API**
  - Copier les services de DI depuis le MVC Program.cs
  - Configurer les controllers API (`AddControllers()` au lieu de `AddControllersWithViews()`)
  - Configurer CORS pour le client frontend
  - Ajouter Swagger/OpenAPI
  - Configurer JWT Authentication (remplacer Cookie Auth)

- [ ] **2.2 Configurer appsettings.json**
  - Copier la chaîne de connexion
  - Copier les paramètres EmailSettings
  - Ajouter les paramètres JWT (Secret, Issuer, Audience)
  - Configurer CORS origins

- [ ] **2.3 Configurer Swagger/OpenAPI**
  ```csharp
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(c => {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });
      // Configuration JWT dans Swagger
  });
  ```

---

### PHASE 3 : Transformation des Controllers 🎮

Pour chaque controller MVC, créer un équivalent API :

#### 3.1 ProductController → ProductsController (API)
- [ ] Créer `Controllers/ProductsController.cs`
- [ ] Transformer les actions :
  | MVC Action | API Endpoint | HTTP Method |
  |------------|--------------|-------------|
  | `ProductIndex()` | `/api/products` | GET |
  | `CreateProduct(dto)` | `/api/products` | POST |
  | `EditProduct(id)` | `/api/products/{id}` | GET |
  | `EditProduct(dto)` | `/api/products/{id}` | PUT |
  | `DeleteProductConfirmed(id)` | `/api/products/{id}` | DELETE |
- [ ] Remplacer les retours `View()` par `Ok()`, `NotFound()`, `BadRequest()`
- [ ] Supprimer les `ViewBag` et `TempData`

#### 3.2 CategoryController → CategoriesController (API)
- [ ] Créer `Controllers/CategoriesController.cs`
- [ ] Endpoints CRUD : GET, POST, PUT, DELETE

#### 3.3 AuthController → AuthController (API)
- [ ] Créer `Controllers/AuthController.cs`
- [ ] Implémenter JWT Authentication :
  - `/api/auth/register` → POST
  - `/api/auth/login` → POST (retourne JWT token)
  - `/api/auth/logout` → POST
  - `/api/auth/refresh-token` → POST
- [ ] Créer les DTOs pour les réponses (LoginResponse avec token)

#### 3.4 CartController → CartController (API)
- [ ] Créer `Controllers/CartController.cs`
- [ ] Endpoints :
  - `/api/cart` → GET (panier utilisateur)
  - `/api/cart/items` → POST (ajouter item)
  - `/api/cart/items/{id}` → PUT/DELETE

#### 3.5 OrderController → OrdersController (API)
- [ ] Créer `Controllers/OrdersController.cs`
- [ ] Endpoints CRUD et actions spéciales

#### 3.6 CouponController → CouponsController (API)
- [ ] Créer `Controllers/CouponsController.cs`
- [ ] Endpoints CRUD + validation

---

### PHASE 4 : Créer les DTOs API 📦

- [ ] **4.1 Créer un dossier `Contracts/` ou réutiliser les DTOs Application**
  - Les DTOs de la couche Application peuvent être réutilisés
  - Créer des DTOs spécifiques API si nécessaire (ex: avec pagination)

- [ ] **4.2 Créer les Response DTOs**
  ```csharp
  public class ApiResponse<T>
  {
      public bool Success { get; set; }
      public string Message { get; set; }
      public T Data { get; set; }
      public List<string> Errors { get; set; }
  }
  ```

- [ ] **4.3 Créer les DTOs d'authentification**
  - `LoginRequestDto` (Email, Password)
  - `LoginResponseDto` (Token, RefreshToken, User)
  - `RegisterRequestDto`

---

### PHASE 5 : Implémenter l'Authentification JWT 🔐

- [ ] **5.1 Installer les packages nécessaires**
  ```bash
  dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
  ```

- [ ] **5.2 Créer le service JWT**
  - Créer `IJwtService` dans `Application/Athentication/Interfaces/`
  - Implémenter `JwtService` dans `Infrastructure/External/`
  - Méthodes : `GenerateToken()`, `ValidateToken()`, `GenerateRefreshToken()`

- [ ] **5.3 Configurer JWT dans Program.cs**
  ```csharp
  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = config["Jwt:Issuer"],
              ValidAudience = config["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(config["Jwt:Secret"]))
          };
      });
  ```

- [ ] **5.4 Sécuriser les endpoints avec `[Authorize]`**

---

### PHASE 6 : Configuration CORS 🌐

- [ ] **6.1 Configurer CORS dans Program.cs**
  ```csharp
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("ClientApp", policy =>
      {
          policy.WithOrigins("http://localhost:4200") // Angular
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
      });
  });
  
  // Dans app.Use...
  app.UseCors("ClientApp");
  ```

---

### PHASE 7 : Gestion des Erreurs et Validation 🚨

- [ ] **7.1 Créer un Global Exception Handler**
  ```csharp
  public class GlobalExceptionMiddleware : IMiddleware
  {
      // Log et retourne une réponse JSON structurée
  }
  ```

- [ ] **7.2 Configurer FluentValidation (optionnel)**
  ```bash
  dotnet add package FluentValidation.AspNetCore
  ```

- [ ] **7.3 Créer des Filters pour la validation**

---

### PHASE 8 : Documentation API 📚

- [ ] **8.1 Ajouter les attributs XML aux endpoints**
  ```csharp
  /// <summary>
  /// Récupère tous les produits
  /// </summary>
  /// <returns>Liste des produits</returns>
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
  public async Task<IActionResult> GetAll()
  ```

- [ ] **8.2 Générer la documentation XML**
  Dans le `.csproj` :
  ```xml
  <PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
  ```

---

### PHASE 9 : Tests de l'API 🧪

- [ ] **9.1 Tester avec Swagger UI**
  - Naviguer vers `/swagger`
  - Tester chaque endpoint

- [ ] **9.2 Tester avec Postman/Insomnia**
  - Créer une collection
  - Tester les flows complets

- [ ] **9.3 Créer des tests d'intégration API**
  ```csharp
  public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
  ```

---

### PHASE 10 : Créer le Projet Client 💻

- [ ] **10.1 Choisir le framework client**
  - Angular (recommandé pour formation)
  - React
  - Blazor WebAssembly
  - Vue.js

- [ ] **10.2 Initialiser le projet client**
  ```bash
  # Pour Angular
  ng new Formation-Ecommerce-Client
  
  # Pour React
  npx create-react-app formation-ecommerce-client
  
  # Pour Blazor
  dotnet new blazorwasm -n Formation-Ecommerce-11-2025.Client
  ```

- [ ] **10.3 Configurer le client pour consommer l'API**
  - Service HTTP
  - Intercepteurs pour JWT
  - Guards de navigation

---

### PHASE 11 : Nettoyage et Finalisation 🧹

- [ ] **11.1 Garder le projet MVC pour référence**
  - Renommer ou archiver

- [ ] **11.2 Mettre à jour la solution**
  - Projet API comme StartUp Project
  - Configurer le lancement multiple (API + Client)

- [ ] **11.3 Mettre à jour la documentation**
  - README.md
  - Architecture diagrams

---

## 📁 Structure Finale de la Solution

```
Formation-Ecommerce-11-2025.sln
├── Formation-Ecommerce-11-2025.Core/           # ✅ INCHANGÉ
├── Formation-Ecommerce-11-2025.Application/    # ✅ INCHANGÉ (ou ajouts mineurs)
├── Formation-Ecommerce-11-2025.Infrastructure/ # ✅ INCHANGÉ (+ JWT Service)
├── Formation-Ecommerce-11-2025.API/            # 🆕 NOUVEAU - Web API
│   ├── Controllers/
│   │   ├── ProductsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── AuthController.cs
│   │   ├── CartController.cs
│   │   ├── OrdersController.cs
│   │   └── CouponsController.cs
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
├── Formation-Ecommerce-11-2025.Client/          # 🆕 NOUVEAU - Frontend
│   └── (Angular/React/Blazor)
└── Formation-Ecommerce-11-2025/                 # 📦 ARCHIVÉ - MVC Original
```

---

## ⏱️ Estimation du Temps

| Phase | Durée estimée |
|-------|---------------|
| Phase 1-2 : Préparation | 1-2 heures |
| Phase 3 : Controllers API | 3-4 heures |
| Phase 4 : DTOs | 1 heure |
| Phase 5 : JWT Auth | 2-3 heures |
| Phase 6-8 : CORS, Erreurs, Doc | 2 heures |
| Phase 9 : Tests | 2 heures |
| Phase 10 : Client | 4-8 heures (selon framework) |
| **TOTAL** | **15-22 heures** |

---

## 💡 Conseils pour la Formation

1. **Montrer le parallèle** entre les actions MVC et les endpoints API
2. **Démontrer l'avantage** : le même backend peut servir Web, Mobile, Desktop
3. **Insister sur les DTOs** : ils protègent les entités et permettent la flexibilité
4. **Atelier pratique** : faire transformer un controller ensemble

---

## 🔗 Ressources

- [ASP.NET Core Web API Documentation](https://docs.microsoft.com/aspnet/core/web-api)
- [JWT Authentication in ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/authentication)
- [Swagger/OpenAPI](https://docs.microsoft.com/aspnet/core/tutorials/web-api-help-pages-using-swagger)

---

# 💻 CODE D'IMPLÉMENTATION COMPLET

Cette section contient le code complet pour implémenter l'API REST.

---

## 📁 Configuration du Projet

### appsettings.json

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "ConnectionStrings": {
        "DefaultConnection": "Server=.; Database=Formation-Ecommerce-11-2025; User Id=sa; Password=sa123456; TrustServerCertificate=True"
    },
    "EmailSettings": {
        "SmtpServer": "smtp.gmail.com",
        "SmtpPort": 587,
        "SmtpUsername": "formationecommerce2025@gmail.com",
        "SmtpPassword": "hqkw bgja cdly pvnw",
        "SenderName": "Formation Ecommerce",
        "SenderEmail": "formationecommerce2025@gmail.com",
        "AuthenticationType": "Plain",
        "EnableSsl": true,
        "BaseUrl": "http://localhost:5000"
    },
    "JwtSettings": {
        "Secret": "YourSuperSecretKeyForJWTTokenGeneration2025!@#$%",
        "Issuer": "Formation-Ecommerce-API",
        "Audience": "Formation-Ecommerce-Client",
        "ExpirationMinutes": 60
    },
    "AllowedHosts": "*"
}
```

### Program.cs

```csharp
using Formation_Ecommerce_11_2025.Infrastructure.Extension;
using Formation_Ecommerce_11_2025.Application.Extension;
using Formation_Ecommerce_11_2025.Infrastructure.Persistence;
using Formation_Ecommerce_11_2025.Core.Entities.Identity;
using Formation_Ecommerce_11_2025.Core.Not_Mapped_Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(option => 
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Email Settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Dependency Injection from layers
builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"] ?? "DefaultSecretKeyForDevelopmentOnly12345";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "Formation-Ecommerce-API",
        ValidAudience = jwtSettings["Audience"] ?? "Formation-Ecommerce-Client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "E-Commerce API", 
        Version = "v1",
        Description = "API REST pour l'application E-Commerce Formation"
    });
    
    // JWT Security Definition for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

## 🎮 Controllers API

### ProductsController.cs

```csharp
using Formation_Ecommerce_11_2025.Application.Products.Dtos;
using Formation_Ecommerce_11_2025.Application.Products.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        /// <summary>
        /// Récupère tous les produits
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productServices.ReadAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Récupère un produit par son ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productServices.ReadByIdAsync(id);
            if (product == null)
                return NotFound(new { Message = "Produit non trouvé" });
            
            return Ok(product);
        }

        /// <summary>
        /// Crée un nouveau produit
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productServices.AddAsync(createProductDto);
            if (result == null)
                return BadRequest(new { Message = "Erreur lors de la création du produit" });

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Met à jour un produit existant
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateProductDto.Id)
                return BadRequest(new { Message = "L'ID ne correspond pas" });

            var existing = await _productServices.ReadByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = "Produit non trouvé" });

            await _productServices.UpdateAsync(updateProductDto);
            return NoContent();
        }

        /// <summary>
        /// Supprime un produit
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _productServices.ReadByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = "Produit non trouvé" });

            await _productServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
```

### CategoriesController.cs

```csharp
using Formation_Ecommerce_11_2025.Application.Categories.Dtos;
using Formation_Ecommerce_11_2025.Application.Categories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.ReadAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.ReadByIdAsync(id);
            if (category == null)
                return NotFound(new { Message = "Catégorie non trouvée" });
            
            return Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryService.AddAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateCategoryDto.Id)
                return BadRequest(new { Message = "L'ID ne correspond pas" });

            await _categoryService.UpdateAsync(updateCategoryDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
```

### AuthController.cs (avec JWT)

```csharp
using Formation_Ecommerce_11_2025.Application.Athentication.Dtos;
using Formation_Ecommerce_11_2025.Application.Athentication.Interfaces;
using Formation_Ecommerce_11_2025.Core.Entities.Identity;
using Formation_Ecommerce_11_2025.Core.Interfaces.External.Mailing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            IAuthService authService,
            IEmailSender emailSender,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _emailSender = emailSender;
            _configuration = configuration;
            _userManager = userManager;
        }

        /// <summary>
        /// Inscription d'un nouvel utilisateur
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Register(registerDto);
            
            if (result == "Inscription réussie!")
            {
                var user = await _authService.GetUserByEmail(registerDto.Email);
                var token = await _authService.GenerateEmailConfirmationToken(user.Id);
                await _emailSender.SendEmailConfirmationAsync(registerDto.Email, token, user.Id);
                
                return Ok(new { Message = "Inscription réussie. Vérifiez votre email." });
            }

            return BadRequest(new { Message = result });
        }

        /// <summary>
        /// Connexion - Retourne un JWT Token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailConfirmed = await _authService.CheckConfirmedEmail(loginDto.Email);
            if (emailConfirmed == null)
                return Unauthorized(new { Message = "Identifiants invalides" });
            
            if (emailConfirmed == false)
                return Unauthorized(new { Message = "Email non confirmé" });

            var loginResult = await _authService.Login(loginDto);
            if (!loginResult.IsSuccess)
                return Unauthorized(new { Message = "Identifiants invalides" });

            // Générer le JWT Token
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"))
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                return BadRequest(new { Message = "Lien invalide" });

            var result = await _authService.ConfirmEmail(userId, token);
            if (result)
                return Ok(new { Message = "Email confirmé" });

            return BadRequest(new { Message = "Échec de la confirmation" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _authService.GetUserByEmail(dto.Email);
            if (user != null)
            {
                var token = await _authService.GeneratePasswordResetToken(user.Id);
                await _emailSender.SendPasswordResetEmailAsync(dto.Email, token, user.Id);
            }
            return Ok(new { Message = "Si votre email existe, vous recevrez un lien." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPassword(dto.UserId, dto.Token, dto.NewPassword);
            if (result)
                return Ok(new { Message = "Mot de passe réinitialisé" });
            return BadRequest(new { Message = "Échec de la réinitialisation" });
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? "DefaultSecretKey12345";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTOs Réponse
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }
}
```

### CartController.cs

```csharp
using Formation_Ecommerce_11_2025.Application.Cart.Dtos;
using Formation_Ecommerce_11_2025.Application.Cart.Interfaces;
using Formation_Ecommerce_11_2025.Application.Coupons.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(ICartService cartService, ICouponService couponService)
        {
            _cartService = cartService;
            _couponService = couponService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cart ?? new CartDto());
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            addToCartDto.UserId = GetUserId();
            var result = await _cartService.AddToCartAsync(addToCartDto);
            if (result == null)
                return BadRequest(new { Message = "Erreur" });
            return Ok(result);
        }

        [HttpPut("items/{cartDetailsId:guid}")]
        public async Task<IActionResult> UpdateQuantity(Guid cartDetailsId, [FromBody] UpdateCartItemDto dto)
        {
            var result = await _cartService.UpdateCartItemQuantityAsync(cartDetailsId, dto.Quantity);
            return Ok(result);
        }

        [HttpDelete("items/{cartDetailsId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid cartDetailsId)
        {
            await _cartService.RemoveCartItemAsync(cartDetailsId);
            return NoContent();
        }

        [HttpPost("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponDto dto)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(dto.CouponCode);
            if (coupon == null)
                return BadRequest(new { Message = "Coupon invalide" });

            var cart = await _cartService.ApplyCouponAsync(GetUserId(), dto.CouponCode);
            return Ok(cart);
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync(GetUserId());
            return NoContent();
        }
    }

    public class AddToCartDto { public string UserId { get; set; } public Guid ProductId { get; set; } public int Count { get; set; } = 1; }
    public class UpdateCartItemDto { public int Quantity { get; set; } }
    public class ApplyCouponDto { public string CouponCode { get; set; } }
}
```

### OrdersController.cs

```csharp
using Formation_Ecommerce_11_2025.Application.Orders.Dtos;
using Formation_Ecommerce_11_2025.Application.Orders.Interfaces;
using Formation_Ecommerce_11_2025.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderServices.GetOrdersByUserIdAsync(GetUserId());
            return Ok(orders);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var orderHeader = new OrderHeaderDto
            {
                UserId = GetUserId(),
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Status = StaticDetails.Status_Pending,
                OrderTime = DateTime.Now,
                OrderTotal = dto.OrderTotal,
                OrderDetails = dto.OrderDetails
            };

            var result = await _orderServices.AddOrderHeaderAsync(orderHeader);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
        {
            await _orderServices.UpdateOrderStatusAsync(id, dto.Status);
            return NoContent();
        }

        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            if (order?.Status != StaticDetails.Status_Pending)
                return BadRequest(new { Message = "Impossible d'annuler" });

            await _orderServices.UpdateOrderStatusAsync(id, StaticDetails.Status_Cancelled);
            return NoContent();
        }
    }

    public class CreateOrderDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal OrderTotal { get; set; }
        public List<OrderDetailsDto> OrderDetails { get; set; } = new();
    }

    public class UpdateOrderStatusDto { public string Status { get; set; } }
}
```

### CouponsController.cs

```csharp
using Formation_Ecommerce_11_2025.Application.Coupons.Dtos;
using Formation_Ecommerce_11_2025.Application.Coupons.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return Ok(coupons);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var coupon = await _couponService.GetCouponByIdAsync(id);
            if (coupon == null)
                return NotFound();
            return Ok(coupon);
        }

        [HttpGet("validate/{code}")]
        [Authorize]
        public async Task<IActionResult> ValidateCoupon(string code)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
                return NotFound(new { Message = "Coupon invalide" });
            return Ok(coupon);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCouponDto dto)
        {
            var result = await _couponService.CreateCouponAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCouponDto dto)
        {
            await _couponService.UpdateCouponAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _couponService.DeleteCouponAsync(id);
            return NoContent();
        }
    }
}
```

---

## 🧪 Test des Endpoints

### Endpoints disponibles :

| Controller | Method | Endpoint | Description |
|------------|--------|----------|-------------|
| **Products** | GET | `/api/products` | Liste des produits |
| | GET | `/api/products/{id}` | Détail d'un produit |
| | POST | `/api/products` | Créer un produit |
| | PUT | `/api/products/{id}` | Modifier un produit |
| | DELETE | `/api/products/{id}` | Supprimer un produit |
| **Categories** | GET | `/api/categories` | Liste des catégories |
| | GET | `/api/categories/{id}` | Détail d'une catégorie |
| | POST | `/api/categories` | Créer une catégorie |
| | PUT | `/api/categories/{id}` | Modifier une catégorie |
| | DELETE | `/api/categories/{id}` | Supprimer une catégorie |
| **Auth** | POST | `/api/auth/register` | Inscription |
| | POST | `/api/auth/login` | Connexion (retourne JWT) |
| | GET | `/api/auth/confirm-email` | Confirmer email |
| | POST | `/api/auth/forgot-password` | Mot de passe oublié |
| | POST | `/api/auth/reset-password` | Réinitialiser MDP |
| **Cart** | GET | `/api/cart` | Voir le panier |
| | POST | `/api/cart/items` | Ajouter au panier |
| | PUT | `/api/cart/items/{id}` | Modifier quantité |
| | DELETE | `/api/cart/items/{id}` | Retirer du panier |
| | POST | `/api/cart/apply-coupon` | Appliquer coupon |
| | DELETE | `/api/cart` | Vider le panier |
| **Orders** | GET | `/api/orders` | Mes commandes |
| | GET | `/api/orders/{id}` | Détail commande |
| | POST | `/api/orders` | Créer commande |
| | PUT | `/api/orders/{id}/status` | Changer statut (Admin) |
| | POST | `/api/orders/{id}/cancel` | Annuler commande |
| **Coupons** | GET | `/api/coupons` | Liste (Admin) |
| | GET | `/api/coupons/{id}` | Détail (Admin) |
| | GET | `/api/coupons/validate/{code}` | Valider code |
| | POST | `/api/coupons` | Créer (Admin) |
| | PUT | `/api/coupons/{id}` | Modifier (Admin) |
| | DELETE | `/api/coupons/{id}` | Supprimer (Admin) |

---

## 🚀 Lancer l'API

```bash
cd Formation-Ecommerce-11-2025.API
dotnet run
```

Accédez à Swagger UI : `http://localhost:5000/swagger`
