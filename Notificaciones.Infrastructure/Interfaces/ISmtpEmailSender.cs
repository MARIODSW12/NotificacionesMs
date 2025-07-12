using System.Net.Mail;

namespace Notificaciones.Infrastructure.Interfaces
{
    public interface ISmtpEmailSender
    {
        Task SendMailAsync(MailMessage mailMessage);
    }
}
