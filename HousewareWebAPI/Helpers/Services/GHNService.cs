using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IGHNService
    {
        public JObject RegisterShop(GHNRegisterShopRequest model);
        public JObject CalculateFee(GHNCalculateFeeRequest model);
        public JObject CreateOrder(GHNCreateOrderRequest model, int shopId);
        public JObject PreviewOrder(GHNCreateOrderRequest model, int shopId);
        public JObject GetOrder(string orderId);
    }

    public class GHNService : IGHNService
    {
        private readonly AppSettings _appSettings;

        public GHNService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public JObject RegisterShop(GHNRegisterShopRequest model)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shop/register");
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Token", _appSettings.GHNToken);
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string resultJson;

            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JObject.FromObject(model));
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultJson = streamReader.ReadToEnd();
            }

            var result = JsonConvert.DeserializeObject<GHNResponse>(resultJson);

            return result.Data;
        }

        public JObject CalculateFee(GHNCalculateFeeRequest model)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee");
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Token", _appSettings.GHNToken);
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string resultJson;

            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JObject.FromObject(model));
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultJson = streamReader.ReadToEnd();
            }

            var result = JsonConvert.DeserializeObject<GHNResponse>(resultJson);

            if (result.Code == 200)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        public JObject CreateOrder(GHNCreateOrderRequest model, int shopId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create");
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Token", _appSettings.GHNToken);
            httpWebRequest.Headers.Add("ShopId", shopId.ToString());
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string resultJson;

            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JObject.FromObject(model));
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultJson = streamReader.ReadToEnd();
            }

            var result = JsonConvert.DeserializeObject<GHNResponse>(resultJson);

            if (result.Code == 200)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        public JObject PreviewOrder(GHNCreateOrderRequest model, int shopId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/preview");
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Token", _appSettings.GHNToken);
            httpWebRequest.Headers.Add("ShopId", shopId.ToString());
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string resultJson;

            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JObject.FromObject(model));
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultJson = streamReader.ReadToEnd();
            }

            var result = JsonConvert.DeserializeObject<GHNResponse>(resultJson);

            if (result.Code == 200)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        public JObject GetOrder(string orderId)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail-by-client-code");
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Token", _appSettings.GHNToken);
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string resultJson;
            JObject model = new();
            model.Add("client_order_code", orderId.ToLower());
            using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(model);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                resultJson = streamReader.ReadToEnd();
            }

            var result = JsonConvert.DeserializeObject<GHNResponse>(resultJson);

            if (result.Code == 200)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
    }
}
