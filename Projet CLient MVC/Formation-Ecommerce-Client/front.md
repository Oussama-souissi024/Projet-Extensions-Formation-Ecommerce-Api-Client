# Checklist Front (Client MVC) - Consommation API (JWT Cookie)

## 1) Configuration de base du Client MVC
- [ ] **Définir l’URL de l’API** dans `appsettings.json` (ex: `ApiBaseUrl`).
- [ ] **Configurer l’accès HTTP** (ex: `HttpClient`) dans `Program.cs`.
- [ ] **Centraliser les routes API** (ex: constantes ou options) pour éviter les URLs en dur.

## 2) Stratégie JWT (Option B) : Cookie “non-auth”
- [ ] **Stocker le JWT dans un cookie** après le login.
- [ ] **Configurer le cookie** avec :
  - [ ] `HttpOnly = true`
  - [ ] `Secure = true` (en HTTPS)
  - [ ] `SameSite = Lax` (recommandé pour débutants)
  - [ ] `Expires` aligné avec `expiresAt` / durée du token
- [ ] **Supprimer le cookie** au logout.

## 3) Auth (UI MVC) - Approche débutant
### Niveau 1 (simple) : UI sans `[Authorize]`
- [ ] **Autorisation gérée par l’API uniquement**.
- [ ] **Sur 401/403** :
  - [ ] Rediriger vers `Auth/Login` (ou page AccessDenied)
  - [ ] Supprimer le cookie JWT si token invalide/expiré

### Niveau 2 (optionnel) : `[Authorize]` côté MVC + rôles
- [ ] **Créer une auth locale MVC (cookie auth)** après login, à partir des claims/roles.
- [ ] **Protéger les controllers/actions** avec `[Authorize]` / `[Authorize(Roles = "Admin")]`.
- [ ] **Garder l’API comme source de vérité** (l’API doit rester sécurisée).

## 4) Services HTTP (dossier `Services`)
- [ ] **Un service par “ressource” API** (ex: `ProductService`, `CartService`, `OrderService`, `AuthService`).
- [ ] **Lire le token depuis le cookie** à chaque appel API.
- [ ] **Envoyer l’en-tête** `Authorization: Bearer <token>` sur les endpoints protégés.
- [ ] **Gérer les erreurs** de manière standard :
  - [ ] Si réponse = 401/403 → redirection/login
  - [ ] Si réponse = 400/404/500 → remonter un message lisible (TempData)
- [ ] **Respecter les formats attendus par l’API** :
  - [ ] JSON (`application/json`) pour la plupart des appels
  - [ ] `multipart/form-data` pour les créations/modifications avec image (produits)

## 5) Maintien de l’expérience utilisateur (Views identiques)
- [ ] **Conserver les Views existantes** (objectif : UI identique).
- [ ] **Conserver les ViewModels** utilisés par les Views.
- [ ] **Adapter uniquement les Controllers MVC** pour appeler les `Services` API.

## 6) Flow Catalogue / Produits
- [ ] **Lister produits** : MVC appelle `GET /api/products`.
- [ ] **Détails produit** : MVC appelle `GET /api/products/{id}`.
- [ ] **Création / Modification** (Admin) :
  - [ ] MVC envoie `multipart/form-data` vers `POST/PUT /api/products`.
  - [ ] Ajouter le token dans la requête.

## 7) Flow Panier (Cart)
- [ ] **Afficher panier** : MVC appelle `GET /api/cart` (token requis).
- [ ] **Ajouter / mettre à jour panier** : MVC appelle `POST /api/cart` (token requis).
- [ ] **Supprimer un item** : MVC appelle `DELETE /api/cart/items/{cartDetailsId}` (token requis).
- [ ] **Appliquer coupon** : MVC appelle `POST /api/cart/apply-coupon` (token requis).
- [ ] **Retirer coupon** : MVC appelle `POST /api/cart/remove-coupon` (token requis).
- [ ] **Vider panier** : MVC appelle `DELETE /api/cart` (token requis).

## 8) Flow Checkout (confirmé : identique à l’ancien MVC, mais via API)
- [ ] **Page Checkout (GET)** :
  - [ ] Récupérer le panier via `GET /api/cart`
  - [ ] Afficher la vue `Checkout` identique
- [ ] **Validation Checkout (POST)** :
  - [ ] Construire la commande côté MVC à partir du modèle posté
  - [ ] Appeler `POST /api/orders` (token requis)
  - [ ] En cas de succès :
    - [ ] Vider panier via `DELETE /api/cart` (si l’API ne le fait pas déjà)
    - [ ] Rediriger vers confirmation

## 9) Points d’attention (sécurité / cohérence)
- [ ] **Ne pas faire confiance au client** pour les prix/totaux :
  - [ ] L’API doit recalculer (ou au minimum valider) les montants à partir de la DB.
- [ ] **Gérer l’expiration du token** :
  - [ ] Si token expiré → supprimer cookie + rediriger login
- [ ] **Aligner les URLs email** :
  - [ ] Vérifier `EmailSettings:BaseUrl` (confirm/reset) selon l’app qui gère la page de confirmation.

