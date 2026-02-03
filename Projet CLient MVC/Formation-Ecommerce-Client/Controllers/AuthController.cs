using Formation_Ecommerce_11_2025.Models.Auth;
using Formation_Ecommerce_Client.Models.ViewModels.Auth;
using Formation_Ecommerce_Client.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Formation_Ecommerce_Client.Controllers
{
    /// <summary>
    /// Contrôleur MVC côté Client : gère les pages d'authentification (login/inscription) et orchestre les appels à l'API.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Ici, le contrôleur ne déclenche pas directement le pipeline ASP.NET Identity (cookies) ; il délègue à <see cref="IAuthApiService"/>.
    /// - Les identifiants sont envoyés à l'API, qui renvoie un JWT ; le stockage du token est géré par le service HTTP (Session).
    /// - Le contrôleur reste centré sur l'UI (ModelState/TempData/Redirections) et non sur la sécurité serveur.
    /// </remarks>
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authService;

        public AuthController(IAuthApiService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            try
            {
                var tokenResponse = await _authService.LoginAsync(model);
                TempData["Success"] = "Connexion réussie!";
                
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                     return Redirect(returnUrl);
                else
                     return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                ViewData["ReturnUrl"] = returnUrl;
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text="Administrateur",Value="Admin"},
                new SelectListItem{Text="Client",Value="Customer"},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var roleList = new List<SelectListItem>()
                {
                    new SelectListItem{Text="Administrateur",Value="Admin"},
                    new SelectListItem{Text="Client",Value="Customer"},
                };
                ViewBag.RoleList = roleList;
                return View(model);
            }

            try
            {
                var result = await _authService.RegisterAsync(model);
                if (result)
                {
                    TempData["Success"] = "Inscription réussie! Veuillez vous connecter.";
                    return RedirectToAction(nameof(Login));
                }
                
                TempData["error"] = "Échec de l'inscription.";
                ModelState.AddModelError("", "Échec de l'inscription.");
                var roleList = new List<SelectListItem>()
                {
                    new SelectListItem{Text="Administrateur",Value="Admin"},
                    new SelectListItem{Text="Client",Value="Customer"},
                };
                ViewBag.RoleList = roleList;
                return View(model);
            }
            catch (Exception ex)
            {
                 TempData["error"] = ex.Message;
                 ModelState.AddModelError("", ex.Message);
                 var roleList = new List<SelectListItem>()
                {
                    new SelectListItem{Text="Administrateur",Value="Admin"},
                    new SelectListItem{Text="Client",Value="Customer"},
                };
                ViewBag.RoleList = roleList;
                 return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ForgotPasswordAsync(model.Email);
                if (result)
                {
                    TempData["Success"] = "Si votre email existe, vous recevrez un lien de réinitialisation.";
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Lien de confirmation invalide.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _authService.ConfirmEmailAsync(userId, token);
            if (result)
            {
                TempData["Success"] = "Email confirmé avec succès! Vous pouvez maintenant vous connecter.";
            }
            else
            {
                TempData["Error"] = "Échec de la confirmation de l'email.";
            }
            return RedirectToAction(nameof(Login));
        }
        


        [HttpGet]
        public IActionResult ResetPassword(string token, string userId)
        {
            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId)) 
                return RedirectToAction(nameof(Login));

            return View(new ResetPasswordViewModel { Token = token, UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
             if(ModelState.IsValid)
            {
               var result = await _authService.ResetPasswordAsync(model);
               if(result) 
               {
                   TempData["Success"] = "Mot de passe réinitialisé !";
                   return RedirectToAction(nameof(Login));
               }
               else
               {
                   TempData["Error"] = "Erreur lors de la réinitialisation.";
               }
            }
            return View(model);
        }
    }
}
