using System.Net;
using System.Net.Mail;

using Notificaciones.Infrastructure.Interfaces;

namespace Notificaciones.Infrastructure.Services
{
    public class SmtpEmailSenderService : ISmtpEmailSender
    {

        #region SendMailAsync(MailMessage mailMessage)
        public async Task SendMailAsync(MailMessage mailMessage)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException(nameof(mailMessage), "El mensaje de correo no puede ser nulo.");
            }

            var senderEmail = Environment.GetEnvironmentVariable("EMAIL");
            var senderPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderPassword))
            {
                throw new InvalidOperationException("Las credenciales del correo no están configuradas correctamente.");
            }

            using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw new InvalidOperationException($"Error al enviar el correo: {smtpEx.Message}", smtpEx);
            }
        }
        #endregion

    }
}
