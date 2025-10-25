using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Security;

namespace FOOD.SERVICES.MailServices
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _config;
        public EmailServices(IConfiguration config)
        {
                _config=config;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config["EmailSetting:SenderName"],_config["EmailSetting:SenderEmail"]));    
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;    
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            using(var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtp.ConnectAsync(_config["EmailSetting:Server"], int.Parse(_config["EmailSetting:Port"]),SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config["EmailSetting:SenderEmail"], _config["EmailSetting:Password"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
