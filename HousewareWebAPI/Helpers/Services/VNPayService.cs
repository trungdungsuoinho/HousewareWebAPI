using Houseware.WebAPI.Data;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using Microsoft.Extensions.Options;
using System;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IVNPayService
    {
        public string GeneratePaymentURL(string returnUrl, Guid orderId, int amount);
        public bool ValidateSignature(CodeIPNURLRequest model);
    }
    public class VNPayService : IVNPayService
    {
        private readonly HousewareContext _context;
        private readonly AppSettings _appSettings;
        //private readonly HttpContext _httpContext;

        public VNPayService(HousewareContext context, IOptions<AppSettings> appSettings/*, HttpContext httpContext*/)
        {
            _context = context;
            _appSettings = appSettings.Value;
            //_httpContext = httpContext;
        }

        public string GeneratePaymentURL(string returnUrl, Guid orderId, int amount)
        {
            GenPayURLRequest genPayURLRequest = new();
            genPayURLRequest.Vnp_TmnCode = _appSettings.VNP_TmnCode;
            genPayURLRequest.Vnp_Amount = amount*100;
            genPayURLRequest.Vnp_CreateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("N. Central Asia Standard Time"));
            genPayURLRequest.Vnp_IpAddr = "192.168.1.1";//_httpContext.Connection.LocalIpAddress.ToString();
            genPayURLRequest.Vnp_OrderInfo = GlobalVariable.VNPContent(orderId.ToString());
            genPayURLRequest.Vnp_ReturnUrl = returnUrl;
            genPayURLRequest.Vnp_TxnRef = orderId.ToString();

            var url = genPayURLRequest.GenParamRoute();
            string vnp_SecureHash = Utils.HmacSHA512(_appSettings.VNP_HashSecret, url);
            url = _appSettings.VNP_Url + "?" + url + "&vnp_SecureHash=" + vnp_SecureHash;
            return url;
        }

        public bool ValidateSignature(CodeIPNURLRequest model)
        {
            string myChecksum = Utils.HmacSHA512(_appSettings.VNP_HashSecret, model.GenParamRoute());
            return myChecksum.Equals(model.Vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
