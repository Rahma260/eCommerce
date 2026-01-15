using CloudinaryDotNet.Actions;
using eCommerce.Application.DTOs;
using eCommerce.Application.ExternalServices.Interfaces.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;

namespace eCommerce.Application.Services.Implementations.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            //build the email message using MimeMessage,
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(settings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            //connect securely to the SMTP server using TLS,
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                settings.SmtpServer,
                settings.Port,
                SecureSocketOptions.StartTls);

            //authenticate using app credentials,
            await smtp.AuthenticateAsync(settings.Username, settings.Password);

            //send the email asynchronously,
            await smtp.SendAsync(email);

            //and ensure proper resource cleanup.
            await smtp.DisconnectAsync(true);
        }
    }
}
