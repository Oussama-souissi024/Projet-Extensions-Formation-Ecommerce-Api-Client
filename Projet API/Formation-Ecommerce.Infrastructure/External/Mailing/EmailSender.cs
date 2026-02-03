using Formation_Ecommerce_11_2025.Core.Interfaces.External.Mailing;
using Formation_Ecommerce_11_2025.Core.Not_Mapped_Entities;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Formation_Ecommerce_11_2025.Infrastructure.External.Mailing
{
    /// <summary>
    /// Service d'envoi d'emails (SMTP) : envoi générique + emails applicatifs (confirmation, reset password, bienvenue).
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Dans l'architecture "API + Client MVC", les liens (confirmation email / reset password) doivent pointer vers le Client MVC (UI).
    /// - L'API reste la source de vérité (génération des tokens via Identity), mais l'utilisateur clique un lien qui ouvre l'interface Client.
    /// - La configuration SMTP reste en Infrastructure et est injectée via <see cref="IOptions{TOptions}"/>.
    /// </remarks>
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task 
            SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                var email = new MimeMessage();

                //From
                email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));

                //To
                email.To.Add(MailboxAddress.Parse(emailMessage.To));

                //Subject
                email.Subject = emailMessage.Subject;

                //Body
                var builder = new BodyBuilder();
                if (emailMessage.IsHtml)
                {
                    builder.HtmlBody = emailMessage.Body;
                }
                else
                {
                    builder.TextBody = emailMessage.Body;
                }

                //Attachments
                if (emailMessage.Attachments != null && emailMessage.Attachments.Any())
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        if (System.IO.File.Exists(attachment))
                        {
                            builder.Attachments.Add(attachment);
                        }
                    }
                }

                email.Body = builder.ToMessageBody();

                //send email
                using (var client = new SmtpClient())
                {
                    // Utiliser SecureSocketOptions.StartTls pour le port 587
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.SmtpUserName, _emailSettings.SmtpPassword);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // Logging de l'erreur
                Console.WriteLine($"Erreur d'envoi d'email: {ex.Message}");
                throw; // Propager l'exception pour pouvoir la gérer au niveau supérieur
            }
        }
            public async Task SendEmailConfirmationAsync(string email, string confirmationToken, string userId)
        {
            var encodedToken = HttpUtility.UrlEncode(confirmationToken);
            // URL modifiée pour pointer vers notre page HTML de confirmation
            var confirmationLink = $"https://localhost:7064/Auth/ConfirmEmail?token={encodedToken}&userId={userId}";
            var message = new EmailMessage
            {
                To = email,
                Subject = "Confirmation de votre adresse email Formation Test SMTP",
                Body = $@"
                    <html>
                    <body style=""font-family: Arial, sans-serif; line-height: 1.6;"">
                        <div style=""max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e4e4e4; border-radius: 5px;"">

                            <p>Merci <h1 style=""color: #333; text-align: center;"">
                                Confirmation de votre adresse email
                            </h1>de vous être inscrit sur Bienvenue Ecommerce 2025-11!</p>
                            <p>Cliquez sur le bouton ci-dessous pour confirmer votre adresse email:</p>
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href='{confirmationLink}' style=""display: inline-block; background-color: #4CAF50; color: white; padding: 12px 20px; text-decoration: none; border-radius: 4px;"">
                                    Confirmer mon adresse email
                                </a>
                            </div>
                            <p>Ce lien expirera dans 24 heures.</p>
                            <div style=""margin-top: 30px; padding-top: 20px; border-top: 1px solid #e4e4e4;"">
                                <p>Cordialement,<br>L'équipe Bienvenue Ecommerce 2025-11</p>
                            </div>
                        </div>
                    </body>
                    </html>",
                IsHtml = true
            };

            // Envoyer l'email
            await SendEmailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken, string userId)
        {
            var encodedToken = HttpUtility.UrlEncode(resetToken);
            var resetLink = $"https://localhost:7064/Auth/ResetPassword?token={encodedToken}&userId={userId}";
            var message = new EmailMessage
            {
                To = email,
                Subject = "Réinitialisation de votre mot de passe",
                Body = $@"
                    <html>
                    <body style=""font-family: Arial, sans-serif; line-height: 1.6;"">
                        <div style=""max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e4e4e4; border-radius: 5px;"">
                            <h1 style=""color: #333; text-align: center;"">
                                Réinitialisation de votre mot de passe
                            </h1>
                            <p>Vous avez demandé la réinitialisation de votre mot de passe.</p>
                            <p>Cliquez sur le bouton ci-dessous pour définir un nouveau mot de passe:</p>
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href='{resetLink}' style=""display: inline-block; background-color: #4CAF50; color: white; padding: 12px 20px; text-decoration: none; border-radius: 4px;"">
                                    Réinitialiser mon mot de passe
                                </a>
                            </div>
                            <p>Ce lien expirera dans 24 heures.</p>
                            <p>Si vous n'avez pas demandé cette réinitialisation, vous pouvez ignorer cet email.</p>
                            <div style=""margin-top: 30px; padding-top: 20px; border-top: 1px solid #e4e4e4;"">
                                <p>Cordialement,<br>L'équipe Bienvenue Ecommerce 2025-11</p>
                            </div>
                        </div>
                    </body>
                    </html>",
                IsHtml = true
            };

            // Envoyer l'email
            await SendEmailAsync(message);
        }

        public async Task SendWelcomeEmailAsync(string email, string username)
        {
            var message = new EmailMessage
            {
                To = email,
                Subject = "Bienvenue Ecommerce 2025-11!",
                Body = $@"
                    <html>
                    <body style=""font-family: Arial, sans-serif; line-height: 1.6;"">
                        <div style=""max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e4e4e4; border-radius: 5px;"">
                            <h1 style=""color: #333; text-align: center;"">
                                Bienvenue sur Bienvenue Ecommerce 2025-11, {username}!
                            </h1>
                            <p>Nous sommes ravis de vous compter parmi nos utilisateurs.</p>
                            <p>Votre compte a été confirmé avec succès et est maintenant actif.</p>
                            <p>Voici ce que vous pouvez faire maintenant:</p>
                            <ul>
                                <li>Explorer notre catalogue de produits</li>
                                <li>Créer votre liste de souhaits</li>
                                <li>Passer votre première commande</li>
                            </ul>
                            <div style=""text-align: center; margin: 30px 0;"">
                                <a href='https://localhost:7064/' style=""display: inline-block; background-color: #4CAF50; color: white; padding: 12px 20px; text-decoration: none; border-radius: 4px;"">
                                    Commencer mon expérience
                                </a>
                            </div>
                            <div style=""margin-top: 30px; padding-top: 20px; border-top: 1px solid #e4e4e4;"">
                                <p>Cordialement,<br>L'équipe Bienvenue Ecommerce 2025-11</p>
                            </div>
                        </div>
                    </body>
                    </html>",
                IsHtml = true
            };

            // Envoyer l'email
            await SendEmailAsync(message);
        }

    }
}

