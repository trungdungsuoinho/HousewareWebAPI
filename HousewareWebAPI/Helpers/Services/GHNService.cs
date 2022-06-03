using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IGHNService
    {
        public JObject RegisterShop(GHNRegisterShopRequest model);
        public JObject CalculateFee(GHNCalculateFeeRequest model);
        public Response GetProvince();
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

            return result.Data;
        }

        public Response GetProvince()
        {
            Response response = new();
            var request = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province");
            request.Method = "POST";
            request.Headers.Add(string.Format("Token: {0}", _appSettings.GHNToken));
            request.ContentType = "application/json";
            using var responseResult = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(responseResult.GetResponseStream()).ReadToEnd();
            var responseJson = JsonConvert.DeserializeObject<JObject>(responseString);
            response.SetCode(CodeTypes.Success);
            response.SetResult(responseJson.GetValue("data"));
            return response;
        }
    }
}
