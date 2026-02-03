using Formation_Ecommerce_11_2025.Application.Athentication.Dtos;
using Formation_Ecommerce_11_2025.Application.Athentication.Interfaces;
using Formation_Ecommerce_11_2025.API.Models;
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
    /// <summary>
    /// Contrôleur Web API d'authentification : expose les endpoints REST d'inscription, de connexion et de gestion des tokens (email/MDP).
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le contrôleur MVC du projet monolithique :
    /// - Ici, pas de vues Razor : chaque action renvoie du JSON (souvent via <see cref="ApiResponse{T}"/>) et des codes HTTP.
    /// - La connexion retourne un JWT (Bearer) que le client (MVC séparé, mobile, SPA, etc.) utilisera dans l'en-tête Authorization.
    /// - Les workflows confirmation email / reset password restent identiques côté "métier" (Application/Identity), seule la présentation change.
    /// </remarks>
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
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation échouée",
                    Errors = errors
                });
            }

            var result = await _authService.Register(registerDto);
            
            if (result == "Inscription réussie!")
            {
                var user = await _authService.GetUserByEmail(registerDto.Email);
                var token = await _authService.GenerateEmailConfirmationToken(user.Id);
                await _emailSender.SendEmailConfirmationAsync(registerDto.Email, token, user.Id);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Inscription réussie. Veuillez vérifier votre email pour confirmer votre compte."
                });
            }

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = result
            });
        }

        /// <summary>
        /// Connexion utilisateur - Retourne un JWT Token
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<JwtLoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation échouée",
                    Errors = errors
                });
            }

            // Vérifier si l'email est confirmé
            var emailConfirmed = await _authService.CheckConfirmedEmail(loginDto.Email);
            if (emailConfirmed == null)
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Identifiants invalides"
                });
            
            if (emailConfirmed == false)
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Votre email n'est pas encore confirmé"
                });

            // Tenter la connexion
            var loginResult = await _authService.Login(loginDto);
            if (!loginResult.IsSuccess)
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Identifiants invalides"
                });

            // Générer le JWT Token
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Identifiants invalides"
                });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return Ok(new ApiResponse<JwtLoginResponseDto>
            {
                Success = true,
                Data = new JwtLoginResponseDto
                {
                    Token = token,
                    Email = user.Email ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    Roles = roles.ToList(),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(
                        int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"))
                }
            });
        }

        /// <summary>
        /// Confirmation de l'email
        /// </summary>
        [HttpGet("confirm-email")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Lien de confirmation invalide"
                });

            var result = await _authService.ConfirmEmail(userId, token);
            if (result)
            {
                var user = await _authService.GetUserById(userId);
                await _emailSender.SendWelcomeEmailAsync(user.Email ?? string.Empty, user.UserName ?? string.Empty);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Email confirmé avec succès"
                });
            }

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Échec de la confirmation de l'email"
            });
        }

        /// <summary>
        /// Demande de réinitialisation de mot de passe
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto forgotPasswordDto)
        {
            if (forgotPasswordDto == null || string.IsNullOrWhiteSpace(forgotPasswordDto.Email))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email requis"
                });
            }

            var user = await _authService.GetUserByEmail(forgotPasswordDto.Email);
            if (user != null)
            {
                var token = await _authService.GeneratePasswordResetToken(user.Id);
                await _emailSender.SendPasswordResetEmailAsync(forgotPasswordDto.Email, token, user.Id);
            }

            // Pour des raisons de sécurité, toujours retourner succès
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Si votre email existe, vous recevrez un lien de réinitialisation."
            });
        }

        /// <summary>
        /// Réinitialisation du mot de passe
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation échouée",
                    Errors = errors
                });
            }

            var result = await _authService.ResetPassword(
                resetPasswordDto.UserId, 
                resetPasswordDto.Token, 
                resetPasswordDto.NewPassword);
            
            if (result)
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Mot de passe réinitialisé avec succès"
                });

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Échec de la réinitialisation du mot de passe"
            });
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"] ?? "DefaultSecretKeyForDevelopmentOnly12345";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Ajouter les rôles aux claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "Formation-Ecommerce-API",
                audience: jwtSettings["Audience"] ?? "Formation-Ecommerce-Client",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
