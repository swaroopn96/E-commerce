using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Ecommerce.Utility
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSettings _mailJetSettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //This method is called from the Register.cshtml.cs file
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {

            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            //Copied from https://app.mailjet.com/auth/get_started/developer website
            // MailjetClient parameter keys copied from https://app.mailjet.com/account/api_keys
            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey);
            
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }

            .Property(Send.FromEmail, "swaroop.n02@gmail.com")
            .Property(Send.FromName, "Administrator")
            .Property(Send.Subject, subject)
            .Property(Send.HtmlPart, body)
            .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
                });

            await client.PostAsync(request);
        }
    }
}
