using HousewareWebAPI.Helpers.Common;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HousewareWebAPI.Helpers.Services
{
    public interface ISendGridService
    {
        public void TestSendMail();
    }

    public class SendGridService : ISendGridService
    {
        private readonly AppSettings _appSettings;

        public SendGridService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        //public void SendMail()
        //{
        //    var message = new SendGridMessage();

        //    message.Personalizations = new List<Personalization>(){
        //    new Personalization(){
        //        Tos = new List<EmailAddress>(){
        //            new EmailAddress(){
        //                Email = "john_doe@example.com",
        //                Name = "John Doe"
        //            },
        //            new EmailAddress(){
        //                Email = "julia_doe@example.com",
        //                Name = "Julia Doe"
        //            }
        //        },
        //        Ccs = new List<EmailAddress>(){
        //            new EmailAddress(){
        //                Email = "jane_doe@example.com",
        //                Name = "Jane Doe"
        //            }
        //        },
        //        Bccs = new List<EmailAddress>(){
        //            new EmailAddress(){
        //                Email = "james_doe@example.com",
        //                Name = "Jim Doe"
        //            }
        //        }
        //    },
        //    new Personalization(){
        //        From = new EmailAddress(){
        //            Email = "sales@example.com",
        //            Name = "Example Sales Team"
        //        },
        //        Tos = new List<EmailAddress>(){
        //            new EmailAddress(){
        //                Email = "janice_doe@example.com",
        //                Name = "Janice Doe"
        //            }
        //        },
        //        Bccs = new List<EmailAddress>(){
        //            new EmailAddress(){
        //                Email = "jordan_doe@example.com",
        //                Name = "Jordan Doe"
        //            }
        //        }
        //    }
        //};

        //    message.From = new EmailAddress()
        //    {
        //        Email = "orders@example.com",
        //        Name = "Example Order Confirmation"
        //    };

        //    message.ReplyTo = new EmailAddress()
        //    {
        //        Email = "customer_service@example.com",
        //        Name = "Example Customer Service Team"
        //    };

        //    message.Subject = "Your Example Order Confirmation";

        //    message.Contents = new List<Content>(){
        //    new Content(){
        //        Type = "text/html",
        //        Value = "<p>Hello from Twilio SendGrid!</p><p>Sending with the email service trusted by developers and marketers for <strong>time-savings</strong>, <strong>scalability</strong>, and <strong>delivery expertise</strong>.</p><p>%open-track%</p>"
        //    }
        //};

        //    message.Attachments = new List<Attachment>(){
        //    new Attachment(){
        //        Content = "PCFET0NUWVBFIGh0bWw+CjxodG1sIGxhbmc9ImVuIj4KCiAgICA8aGVhZD4KICAgICAgICA8bWV0YSBjaGFyc2V0PSJVVEYtOCI+CiAgICAgICAgPG1ldGEgaHR0cC1lcXVpdj0iWC1VQS1Db21wYXRpYmxlIiBjb250ZW50PSJJRT1lZGdlIj4KICAgICAgICA8bWV0YSBuYW1lPSJ2aWV3cG9ydCIgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEuMCI+CiAgICAgICAgPHRpdGxlPkRvY3VtZW50PC90aXRsZT4KICAgIDwvaGVhZD4KCiAgICA8Ym9keT4KCiAgICA8L2JvZHk+Cgo8L2h0bWw+Cg==",
        //        Filename = "index.html",
        //        Type = "text/html",
        //        Disposition = "attachment"
        //    }
        //};

        //    message.Categories = new List<string>(){
        //    "cake",
        //    "pie",
        //    "baking"
        //};

        //    message.SendAt = 1617260400;

        //    message.BatchId = "AsdFgHjklQweRTYuIopzXcVBNm0aSDfGHjklmZcVbNMqWert1znmOP2asDFjkl";

        //    message.Asm = new ASM()
        //    {
        //        GroupId = 12345,
        //        GroupsToDisplay = new List<int>(){
        //        12345
        //    }
        //    };

        //    message.IpPoolName = "transactional email";

        //    message.MailSettings = new MailSettings()
        //    {
        //        BypassListManagement = new BypassListManagement()
        //        {
        //            Enable = false
        //        },
        //        FooterSettings = new FooterSettings()
        //        {
        //            Enable = false
        //        },
        //        SandboxMode = new SandboxMode()
        //        {
        //            Enable = false
        //        }
        //    };

        //    message.TrackingSettings = new TrackingSettings()
        //    {
        //        ClickTracking = new ClickTracking()
        //        {
        //            Enable = true,
        //            EnableText = false
        //        },
        //        OpenTracking = new OpenTracking()
        //        {
        //            Enable = true,
        //            SubstitutionTag = "%open-track%"
        //        },
        //        SubscriptionTracking = new SubscriptionTracking()
        //        {
        //            Enable = false
        //        }
        //    };

        //    string apiKey = "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";
        //    var client = new SendGridClient(apiKey);
        //    var response = await client.SendEmailAsync(message).ConfigureAwait(false);

        //    Console.WriteLine(response.StatusCode);
        //    Console.WriteLine(response.Body.ReadAsStringAsync().Result);
        //    Console.WriteLine(response.Headers.ToString());
        //}

        public void TestSendMail()
        {
            Execute().Wait();
        }

        public async Task Execute()
        {
            var apiKey = Utils.Decrypt(_appSettings.SendGridKey);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("houseware.trungdung@gmail.com", "Houseware");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("fxdungvusuoinho@gmail.com", "Trung Dũng");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
