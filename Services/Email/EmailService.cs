using Coursera.Models;
using Coursera.Services.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;


namespace Coursera.Services.Folder
{
    public class EmailService : IEmailService
    {

        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _fromEmail;
        private readonly string _password;


        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            // Initialize SMTP Configuration (Could also be read from appsettings.json)
            var setting = smtpSettings.Value;
            _smtpServer = setting.Server;
            _port = setting.Port;
            _fromEmail = setting.Username;
            _password = setting.Password;
            //_smtpServer = "smtp.gmail.com";
            //_port = 587;
            //_fromEmail = "anandkumarj95@gmail.com";
            //_password = "wjwt rach dqtm nojq";
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_fromEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                // Set email body content
                email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

                // Send email using MailKit
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpServer, _port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_fromEmail, _password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
