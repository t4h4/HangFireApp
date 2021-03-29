using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Web.Services
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Sender(string userId, string message)
        {
            // bu userId'e sahip kullan kullanıcıyı bul ve email adresini al.

            var apiKey = _configuration.GetSection("APIs")["SendGridApi"]; //appsettings.json içindeki api keyi almak için. yukarda DI işlemi de yapıldı.
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("tahayasin@outlook.com", "Example User");
            var subject = "www.mysite.com bilgilendirme";
            var to = new EmailAddress("t4h4@protonmail.com", "Example User");
            //  var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = $"<strong>{message}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
