using HousewareWebAPI.Helpers.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

namespace HousewareWebAPI.Helpers.Services
{
    public interface ITwilioService
    {
        public object SendSMS(string phone, string content);
        public object VerifyPhone(string phone, string content);
    }

    public class TwilioService : ITwilioService
    {
        private readonly AppSettings _appSettings;

        public TwilioService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public object SendSMS(string phone, string content)
        {
            TwilioClient.Init(_appSettings.TwilioAccountSID, _appSettings.TwilioAuthToken);

            var message = MessageResource.Create(
                body: content,
                from: new Twilio.Types.PhoneNumber(_appSettings.TwilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phone)
            );
            return message;
        }

        public object VerifyPhone(string phone, string content)
        {
            TwilioClient.Init(_appSettings.TwilioAccountSID, _appSettings.TwilioAuthToken);

            var verification = VerificationResource.Create(
                to: phone,
                channel: "sms",
                pathServiceSid: _appSettings.TwilioServiceSID
            );
            return verification;
        }
    }
}
