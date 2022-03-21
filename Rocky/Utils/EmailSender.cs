using Microsoft.AspNetCore.Identity.UI.Services;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rocky.Utils
{
    public class EmailSender : IEmailSender
    {
        public System.Threading.Tasks.Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async System.Threading.Tasks.Task Execute(string email, string subject, string body)
        {
            Configuration.Default.ApiKey.Add("api-key", "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");

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "John Doe";
            string SenderEmail = "example@example.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToEmail = "kotkat.lv@gmail.com.com";
            string ToName = "John Doe";
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            string HtmlContent = "<html><body><h1>This is my first transactional email {{params.parameter}}</h1></body></html>";
            string TextContent = null;
            string Subject = "My {{params.subject}}";
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject, null, null, null, null, null, null, null);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Debug.WriteLine(result.ToJson());
                Console.WriteLine(result.ToJson());
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}