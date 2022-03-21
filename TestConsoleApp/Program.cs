using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SendEmailExample
{
    class Program
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.elasticemail.com/v4";
            // Configure API key authorization: apikey
            config.AddApiKey("X-ElasticEmail-ApiKey", "DA7B9060AABC666B1A1C7E5183D1C7B3FE9B43ECA0FE6CA386DC4AA96628A94E0708D8ACDBCB18B4F6B041E7E9764F86");
            // Uncomment below to setup prefix (e.g. Bearer) for API key, if needed
            // config.AddApiKeyPrefix("X-ElasticEmail-ApiKey", "Bearer");

            BodyPart bodyPart = new BodyPart( BodyContentType.HTML, content: "<p>HELLO IT'S ME</p>");
            EmailContent emailContent = new EmailContent(body: new List<BodyPart> { bodyPart }, from: "kotka.lv@gmail.com", replyTo: "kotka.lv@gmail.com");
            var apiInstance = new EmailsApi(config);

            var emailTransactionalMessageData = new EmailTransactionalMessageData
            {
                 Content = emailContent,
            }; // EmailTransactionalMessageData | Email data

            try
            {
                // Send Transactional Email
                EmailSend result = apiInstance.EmailsTransactionalPost(emailTransactionalMessageData);
                Debug.WriteLine(result);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling EmailsApi.EmailsTransactionalPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}