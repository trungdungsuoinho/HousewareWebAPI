using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IGHNService
    {
        public Response RegisterShop(AddStoreRequest model);
        public Response GetProvince();
    }

    public class GHNService : IGHNService
    {
        private readonly AppSettings _appSettings;

        public GHNService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Response RegisterShop(AddStoreRequest model)
        {
            Response response = new();
            var json = new JObject
            {
                { "district_id", model.District },
                { "ward_code", model.Ward },
                { "name", model.Name },
                { "phone", model.Phone },
                { "address", model.Detail }
            };
            string stringData = json.ToString();
            var data = Encoding.Unicode.GetBytes(stringData);

            var request = (HttpWebRequest)WebRequest.Create("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shop/register");
            request.Method = "POST";
            request.Headers.Add(string.Format("Token: {0}", _appSettings.GHNToken));
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var responsea = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(responsea.GetResponseStream()).ReadToEnd();
                var result = JsonConvert.DeserializeObject<JObject>(responseString);
                response.SetCode(CodeTypes.Success);
                response.SetResult(result.GetValue("data"));
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
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
