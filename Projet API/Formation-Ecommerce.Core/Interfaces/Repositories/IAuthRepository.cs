using Formation_Ecommerce_11_2025.Core.Entities.Identity;

namespace Formation_Ecommerce_11_2025.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetUserByUsernameAsync(string username);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<bool> SignOutAsync();
        
        // Méthodes pour la confirmation d'email
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<bool> ConfirmEmailAsync(ApplicationUser user, string token);
        
        // Méthodes pour la réinitialisation de mot de passe
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<bool?> CheckConfirmedEmail(string email);
    }
}
