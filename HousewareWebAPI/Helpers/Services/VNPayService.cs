using Houseware.WebAPI.Data;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IVNPayService
    {
        public string GeneratePaymentURL(string returnUrl, Guid orderId, uint amount);
    }
    public class VNPayService : IVNPayService
    {
        private readonly HousewareContext _context;
        private readonly AppSettings _appSettings;
        //private readonly HttpContext _httpContext;

        private SortedList<string, string> _requestData = new(new VnPayCompare());
        private SortedList<string, string> _responseData = new(new VnPayCompare());

        public VNPayService(HousewareContext context, IOptions<AppSettings> appSettings/*, HttpContext httpContext*/)
        {
            _context = context;
            _appSettings = appSettings.Value;
            //_httpContext = httpContext;
        }

        public string GeneratePaymentURL(string returnUrl, Guid orderId, uint amount)
        {
            GenPayURLRequest genPayURLRequest = new();
            genPayURLRequest.Vnp_TmnCode = _appSettings.VNP_TmnCode;
            genPayURLRequest.Vnp_Amount = (int)amount;
            genPayURLRequest.Vnp_CreateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("N. Central Asia Standard Time"));
            genPayURLRequest.Vnp_IpAddr = "192.168.1.1";//_httpContext.Connection.LocalIpAddress.ToString();
            genPayURLRequest.Vnp_OrderInfo = "Houseware thanh toan don hang " + orderId.ToString();
            genPayURLRequest.Vnp_ReturnUrl = returnUrl;
            genPayURLRequest.Vnp_TxnRef = orderId.ToString();

            return CreateRequestUrl(genPayURLRequest);
        }

        private string CreateRequestUrl(GenPayURLRequest genPayURLRequest)
        {
            var url = genPayURLRequest.GenParamRoute();
            string vnp_SecureHash = Utils.HmacSHA512(_appSettings.VNP_HashSecret, url);
            url = _appSettings.VNP_Url + "?" + url + "&vnp_SecureHash=" + vnp_SecureHash;
            return url;
        }


        public string GetResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }



        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseData()
        {
            StringBuilder data = new();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
