using DATEX_ProjectDatabase.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Replace with your SMTP server
        private readonly int _smtpPort = 587; // Replace with your SMTP port
        private readonly string _smtpUser = "emmatheresajose12.4@gmail.com"; // Replace with your SMTP username
        private readonly string _smtpPass = "sokg bbzh csvz fssv"; // Replace with your SMTP app password

        public async Task SendEmailAsync(string to, string subject, string body)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
