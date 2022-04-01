using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Rocky.Utils
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly MailJetSettings _mailJetSettings;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);
            var emailTarget = new TransactionalEmailBuilder()
               .WithFrom(new SendContact("dotnetmastery_22@protonmail.com"))
               .WithSubject(subject)
               .WithHtmlPart(body)
               .WithTo(new SendContact(email))
               .Build();

            // invoke API to send email
            await client.SendTransactionalEmailAsync(emailTarget);
        }
    }
}