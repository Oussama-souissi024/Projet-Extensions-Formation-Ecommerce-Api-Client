using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_11_2025.Models.Auth
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données du formulaire de réinitialisation du mot de passe.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le token et l'identifiant utilisateur proviennent d'un lien généré par l'API (souvent envoyé par email).
    /// - Le Client MVC affiche le formulaire et renvoie ces données à l'API qui valide le token et applique la modification.
    /// </remarks>
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir au moins {2} caractères et au maximum {1} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et la confirmation du mot de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }
}
