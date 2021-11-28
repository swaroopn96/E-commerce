using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace Ecommerce.Utility
{
    public class EmailSender:IEmailSender
    {
        public EmailSender()
        {
        }

        //This method is called from the Register.cshtml.cs file
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {

            //Copied from https://app.mailjet.com/auth/get_started/developer website
            // MailjetClient parameter keys copied from https://app.mailjet.com/account/api_keys
            MailjetClient client = new MailjetClient("f8272ac3c2b7dfc02612a57c5769ec30", "8e7a0632ec4d2aaf50d167ee28446548");
            
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
