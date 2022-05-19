using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IOAuthService
    {
        public Response GenOAuthURI();
        public Response GetOAuthInfo(string code);
    }

    public class OAuthService : IOAuthService
    {
        private readonly AppSettings _appSettings;

        public OAuthService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Response GenOAuthURI()
        {
            var response = new Response();
            try
            {
                if (string.IsNullOrEmpty(_appSettings.OAuthRedirectURI))
                {
                    response.SetCode(CodeTypes.Err_LackVal);
                    response.SetResult("OAuth redirect URI is null!");
                }
                string score = "https://www.googleapis.com/auth/userinfo.email" + " " +
                               "https://www.googleapis.com/auth/userinfo.profile";
                string uri = string.Format("https://accounts.google.com/o/oauth2/v2/auth" +
                    "?client_id={0}" +
                    "&redirect_uri={1}" +
                    "&response_type=code" +
                    "&score={2}" +
                    "&access_type=offline" +
                    "&include_granted_scopes=true",
                    _appSettings.OAuthClientId,
                    _appSettings.OAuthRedirectURI,
                    score);
                response.SetCode(CodeTypes.Success);
                response.SetResult(new Uri(uri));
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public OAuthTokenRespone GetOAuthToken(string code)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            string param = string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}", code, _appSettings.OAuthClientId, _appSettings.OAuthClientSeret, _appSettings.OAuthRedirectURI);
            var data = Encoding.ASCII.GetBytes(param);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            OAuthTokenRespone result = JsonConvert.DeserializeObject<OAuthTokenRespone>(responseString);
            return result;
        }

        public Response GetOAuthInfo(string code)
        {
            var response = new Response();
            if (code == "access_denied")
            {
                response.SetCode(CodeTypes.Err_AccessDenied);
                response.SetResult("OAuth authentication access denied!");
                return response;
            }

            var oAuthToken = GetOAuthToken(code);

            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/auth/userinfo.profile");
            request.Method = "GET";
            request.Headers.Add(string.Format("Authorization: Bearer {0}", oAuthToken.Access_token));
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";

            using (var responseAPI = (HttpWebResponse)request.GetResponse())
            {
                var responseString = new StreamReader(responseAPI.GetResponseStream()).ReadToEnd();
                response.SetCode(CodeTypes.Success);
                response.SetResult(responseString);
            }
            return response;
        }
    }
}
