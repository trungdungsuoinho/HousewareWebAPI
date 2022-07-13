using HousewareWebAPI.Helpers.Common;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

namespace HousewareWebAPI.Helpers.Services
{
    public interface ITwilioService
    {
        public VerificationResource SendVerification(string phone);
        public VerificationCheckResource CheckVerification(string phone, string code);
        public MessageResource SendSMS(string phone, string content);
    }

    public class TwilioService : ITwilioService
    {
        private readonly AppSettings _appSettings;

        public TwilioService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public VerificationResource SendVerification(string phone)
        {
            TwilioClient.Init(_appSettings.TwilioAccountSID, Utils.Decrypt(_appSettings.TwilioAuthToken));

            var verification = VerificationResource.Create(
                pathServiceSid: _appSettings.TwilioServiceSID,
                to: Utils.ParseInternationalPhoneNumber(phone),
                channel: "sms"
            );
            return verification;
        }

        public VerificationCheckResource CheckVerification(string phone, string code)
        {
            TwilioClient.Init(_appSettings.TwilioAccountSID, Utils.Decrypt(_appSettings.TwilioAuthToken));

            var verificationCheck = VerificationCheckResource.Create(
                pathServiceSid: _appSettings.TwilioServiceSID,
                to: Utils.ParseInternationalPhoneNumber(phone),
                code: code
            );
            return verificationCheck;
        }

        public MessageResource SendSMS(string phone, string content)
        {
            TwilioClient.Init(_appSettings.TwilioAccountSID, Utils.Decrypt(_appSettings.TwilioAuthToken));

            var message = MessageResource.Create(
                body: content,
                from: new Twilio.Types.PhoneNumber(Utils.ParseInternationalPhoneNumber(_appSettings.TwilioPhoneNumber)),
                to: new Twilio.Types.PhoneNumber(Utils.ParseInternationalPhoneNumber(phone))
            );
            return message;
        }


    }
}
