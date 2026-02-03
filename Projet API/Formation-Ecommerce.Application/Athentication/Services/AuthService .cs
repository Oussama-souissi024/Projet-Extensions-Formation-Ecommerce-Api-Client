using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Application.Athentication.Dtos;
using Formation_Ecommerce_11_2025.Application.Athentication.Interfaces;
using Formation_Ecommerce_11_2025.Core.Entities.Identity;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;

namespace Formation_Ecommerce_11_2025.Application.Athentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<bool> AssingnRole(string email, string roleName)
        {
            // Vérifier si l'utilisateur existe
            var user = await _authRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            // Vérifier si le rôle existe, sinon le créer
            if (!await _authRepository.RoleExistsAsync(roleName))
            {
                var roleCreated = await _authRepository.CreateRoleAsync(roleName);
                if (!roleCreated)
                {
                    return false;
                }
            }

            // Attribuer le rôle à l'utilisateur
            var added = await _authRepository.AddUserToRoleAsync(user, roleName);

            return added;
        }

        public async Task<ResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _authRepository.GetUserByEmailAsync(loginRequestDto.Email);

            if (user == null)
                return new ResponseDto { Error = "Utilisateur non trouvé" };

            var signInSucceeded = await _authRepository.PasswordSignInAsync(
                user,
                loginRequestDto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!signInSucceeded)
                return new ResponseDto { Error = "Authentification échouée" };

            return new ResponseDto { IsSuccess = true };
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _authRepository.SignOutAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            // vérifier si l'utilisateur existe déja 
            var existingUser = await _authRepository.GetUserByEmailAsync(registrationRequestDto.Email);
            if (existingUser != null)
            {
                return "Un utilisateur avec cet email existe déjà.";
            }

            // Créer un nouvel utilisateur
            var newUser = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            // Ajouter l'utilisateur
            var created = await _authRepository.CreateUserAsync(newUser, registrationRequestDto.Password);
            if (!created)
            {
                return "Erreur lors de la création de l'utilisateur.";
            }

            string roleName = registrationRequestDto.Role ?? "Customer";
            // Vérifier si le rôle existe, sinon le créer
            if (!await _authRepository.RoleExistsAsync(roleName))
            {
                var roleCreated = await _authRepository.CreateRoleAsync(roleName);
                if (!roleCreated)
                {
                    return "Erreur lors de la création du rôle.";
                }
            }

            var addedToRole = await _authRepository.AddUserToRoleAsync(newUser, roleName);
            if (!addedToRole)
            {
                return "Erreur lors de l'attribution du rôle.";
            }

            return "Inscription réussie!";
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user == null)
                return null;
            return user;

        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                return null;

            return user;

        }

        public async Task<string> GenerateEmailConfirmationToken(string userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                return null;

            return await _authRepository.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            var confirmed = await _authRepository.ConfirmEmailAsync(user, token);
            return confirmed;
        }

        public async Task<string> GeneratePasswordResetToken(string userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                return null;

            return await _authRepository.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPassword(string userId, string token, string newPassword)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            var success = await _authRepository.ResetPasswordAsync(user, token, newPassword);
            return success;
        }

        public async Task<bool?> CheckConfirmedEmail(string email)
        {
            var result = await _authRepository.CheckConfirmedEmail(email);
            if (result == null)
            {
                return null;
            }
            return result;
        }
    }
}

