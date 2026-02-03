using Formation_Ecommerce_11_2025.Core.Not_Mapped_Entities;

namespace Formation_Ecommerce_11_2025.Core.Interfaces.External.Mailing
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage emailMessage);
        Task SendWelcomeEmailAsync(string email, string username);
        Task SendPasswordResetEmailAsync(string email, string resetToken, string userId);
        Task SendEmailConfirmationAsync(string email, string confirmationToken, string userId);
    }
}
