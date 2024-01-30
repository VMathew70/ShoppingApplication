using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace ProductAPI.Models
{
    public class EmailService : IEMailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
           

            var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port);

            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);

            string fromEMail = _mailSettings.Mail == null ? "a.com'" : _mailSettings.Mail;
            string toEmail = mailRequest.ToEmail == null ? "a.com'" : mailRequest.ToEmail;

            MailMessage mailMessage = new MailMessage(from: fromEMail, to: toEmail, mailRequest.Subject, mailRequest.Body);
            await client.SendMailAsync(mailMessage);

        }


    }
}
