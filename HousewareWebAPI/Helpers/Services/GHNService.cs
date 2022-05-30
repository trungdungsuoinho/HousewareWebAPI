using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IGHNService
    {
        public Response GetProvince();
    }

    public class GHNService : IGHNService
    {
        private readonly AppSettings _appSettings;

        public GHNService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
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
