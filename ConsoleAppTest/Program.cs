using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using System;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MailjetClient client = new MailjetClient("1c153b99ca2b95ee49426ac5ef0e8e83", "750b4965229fb21357d5935f4000a3ae");
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            };
            //.Property(Send.Messages, new JArray {
            //    new JObject {
            //         { "From", new JObject { {"Email", "mailjet96@gmail.com"}, {"Name", "John"} }},
            //         { "To", new JArray { new JObject { { "Email", email }, {"Name", "John"}} }}, { "Subject", subject },
            //         { "HTMLPart", body },
            //    }});

            //await client.PostAsync(request);

            var emailTarget = new TransactionalEmailBuilder()
               .WithFrom(new SendContact("mailjet96@gmail.com"))
               .WithSubject("Some subject")
               .WithHtmlPart("Some body")
               .WithTo(new SendContact("kotkat.lv@gmail.com"))
               .Build();

            // invoke API to send email
            await client.SendTransactionalEmailAsync(emailTarget);
        }
    }
}
