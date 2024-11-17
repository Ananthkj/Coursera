using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Coursera.Models
{
    public class EmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            // Ensure the "From" email address is present
            var fromAddress = smtpSettings["Username"]; // Assuming "Username" contains the sender's email address
            if (string.IsNullOrEmpty(fromAddress))
            {
                throw new ArgumentNullException("Username", "The sender's email address is not configured.");
            }

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your App", fromAddress)); // Sender's email address
            emailMessage.To.Add(new MailboxAddress("User", toEmail)); // Recipient's email address
            emailMessage.Body = new TextPart("plain") { Text = message }; // Email body

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpSettings["Server"], int.Parse(smtpSettings["Port"]), MailKit.Security.SecureSocketOptions.StartTls);

            // Ensure that both username and password are provided for authentication
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];
            if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new ArgumentNullException("SMTP Credentials", "SMTP username or password is not configured.");
            }

            await client.AuthenticateAsync(smtpUsername, smtpPassword); // Authenticate with the email provider
            await client.SendAsync(emailMessage); // Send the email
            await client.DisconnectAsync(true); // Disconnect from the server
        }
    }
}
