using HousewareWebAPI.Data.Entities;
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
        public bool SendMail(Customer customer = null, Order order = null);
    }

    public class SendGridService : ISendGridService
    {
        private readonly AppSettings _appSettings;

        public SendGridService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool SendMail(Customer customer = null, Order order = null)
        {
            //if (customer.Email == null || customer.VerifyEmail != "Y")
            //{
            //    return false;
            //}

            var from = new EmailAddress(_appSettings.EmailFrom, _appSettings.NameFrom);
            var subject = "Xác nhận đơn hàng Houseware";
            var to = new EmailAddress("vutrungdung1605@gmail.com", "Trung Dũng");
            var plainTextContent = string.Format("Xác nhận đơn hàng {0}", "abc"/*order.OrderId*/);
            var htmlContent = string.Format("<table cellpadding='0' cellspacing='0' align='left' style='border-collapse:collapse; border - spacing:0px; float:left'><tbody><tr style = 'border-collapse:collapse'><td valign = 'top' align = 'center' style = 'padding:0;margin:0;width:192px'><img src = 'https://res.cloudinary.com/trungdung16/image/upload/v1657651253/avt_ecjskg.png' style = 'display:block;border:0;outline:none;text-decoration:none' width = '192' class='CToWUd'></td></tr></tbody></table>", "abc"/*order.OrderId*/);
            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            sendGridMessage.Categories = new List<string>()
            {
                "order"
            };
            sendGridMessage.SendAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("N. Central Asia Standard Time")).Ticks;
            sendGridMessage.MailSettings = new MailSettings()
            {
                BypassListManagement = new BypassListManagement()
                {
                    Enable = false
                },
                FooterSettings = new FooterSettings()
                {
                    Enable = false
                },
                SandboxMode = new SandboxMode()
                {
                    Enable = false
                }
            };
            sendGridMessage.TrackingSettings = new TrackingSettings()
            {
                ClickTracking = new ClickTracking()
                {
                    Enable = true,
                    EnableText = false
                },
                OpenTracking = new OpenTracking()
                {
                    Enable = true,
                    SubstitutionTag = "%open-track%"
                },
                SubscriptionTracking = new SubscriptionTracking()
                {
                    Enable = false
                }
            };
            //sendGridMessage.Attachments = new List<Attachment>()
            //{
            //    new Attachment(){
            //        Content = "PCFET0NUWVBFIGh0bWw+CjxodG1sIGxhbmc9ImVuIj4KCiAgICA8aGVhZD4KICAgICAgICA8bWV0YSBjaGFyc2V0PSJVVEYtOCI+CiAgICAgICAgPG1ldGEgaHR0cC1lcXVpdj0iWC1VQS1Db21wYXRpYmxlIiBjb250ZW50PSJJRT1lZGdlIj4KICAgICAgICA8bWV0YSBuYW1lPSJ2aWV3cG9ydCIgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEuMCI+CiAgICAgICAgPHRpdGxlPkRvY3VtZW50PC90aXRsZT4KICAgIDwvaGVhZD4KCiAgICA8Ym9keT4KCiAgICA8L2JvZHk+Cgo8L2h0bWw+Cg==",
            //        Filename = "index.html",
            //        Type = "text/html",
            //        Disposition = "attachment"
            //    }
            //};
            Execute(sendGridMessage).Wait();
            return true;
        }

        public async Task<Response> Execute(SendGridMessage sendGridMessage)
        {
            var apiKey = Utils.Decrypt(_appSettings.SendGridKey);
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(sendGridMessage);
            return response;
        }
    }
}
