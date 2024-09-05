using DATEX_ProjectDatabase.Interfaces;
using DotNetEnv;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService()
        {
            // Load environment variables from .env file
            Env.Load();

            _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
            _smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));
            _smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
            _smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(to);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}
