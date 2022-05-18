using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HousewareWebAPI.Controllers
{
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }

    [Route("api/v1/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly string ClientId = "209013346133-9tc2eqpku497obl9k2449jd91rb1h7gn.apps.googleusercontent.com";
        private readonly string ClientSeret = "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";
        private readonly string RedirectURI = "https://localhost:5001/api/v1/oauth/callback";

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(GetAutenticationURI(ClientId, RedirectURI));
        }

        public static Uri GetAutenticationURI(string clientId, string redirectUri)
        {
            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = "urn:ietf:wg:oauth:2.0:oob";
            }
            string oauth = string.Format("https://accounts.google.com/o/oauth2/auth?scope=https://www.googleapis.com/auth/androidpublisher&response_type=code&access_type=offline&redirect_uri={0}&client_id={1}", redirectUri, clientId);
            return new Uri(oauth);
        }

        [HttpGet("callback")]
        public IActionResult Callback([FromQuery] string code)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            string postData = string.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}", code, ClientId, ClientSeret, RedirectURI);
            request.Method = "POST";

            var data = Encoding.ASCII.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            AuthResponse result = JsonConvert.DeserializeObject<AuthResponse>(responseString);

            var request2 = (HttpWebRequest)WebRequest.Create("http://www.googleapis.com/oauth2/v3/userinfo");
            request2.Method = "GET";
            request2.Headers.Add(string.Format("Authorization: Bearer {0}", result.access_token));
            request2.ContentType = "application/x-www-form-urlencoded";
            request2.Accept = "application/json";

            using (var response2 = (HttpWebResponse)request2.GetResponse())
            {
                var responseString2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(responseString2);
                return Ok(array);
            }            
        }
    }
}
