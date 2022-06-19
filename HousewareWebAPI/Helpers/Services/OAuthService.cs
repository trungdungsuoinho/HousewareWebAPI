using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HousewareWebAPI.Helpers.Services
{
    public interface IOAuthService
    {
        public Response GenOAuthURI();
        public Response VerifyEmail(string code);
    }

    public class OAuthService : IOAuthService
    {
        private readonly HousewareContext _context;
        private readonly AppSettings _appSettings;
        private readonly ICustomerService _customerService;

        public OAuthService(HousewareContext context, IOptions<AppSettings> appSettings, ICustomerService customerService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _customerService = customerService;
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
                    "&scope={2}" +
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

        private OAuthToken GetOAuthToken(string code)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            string param = string.Format(
                "grant_type=authorization_code" +
                "&code={0}" +
                "&client_id={1}" +
                "&client_secret={2}" +
                "&redirect_uri={3}", 
                code, 
                _appSettings.OAuthClientId,
                _appSettings.OAuthClientSeret,
                _appSettings.OAuthRedirectURI);
            var data = Encoding.ASCII.GetBytes(param);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var result = JsonConvert.DeserializeObject<OAuthToken>(responseString);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Can't get OAuth token! " + e.Message);
            }            
        }

        private OAuthUserInfo GetOAuthUserInfo(OAuthToken oAuthToken)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/oauth2/v1/userinfo");
            request.Method = "GET";
            request.Headers.Add(string.Format("Authorization: {0} {1}", oAuthToken.Token_type, oAuthToken.Access_token));
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";

            try
            {
                using var response = (HttpWebResponse)request.GetResponse();
                var userInfoString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var userInfo = JsonConvert.DeserializeObject<OAuthUserInfo>(userInfoString);
                return userInfo;
            }
            catch (Exception e)
            {
                throw new Exception("Can't get Email from this OAuth token! " + e.Message);
            }
        }

        private Customer GetCusByEmail(string email)
        {
            return _context.Customers.Where(c => c.Email == email).FirstOrDefault();
        }

        public Customer Register(OAuthUserInfo model)
        {
            try
            {
                Customer customer = new()
                {
                    Email = model.Email,
                    VerifyEmail = "Y",
                    FullName = model.Name,
                    Picture = model.Picture,
                };
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return GetCusByEmail(model.Email);
            }
            catch (Exception e)
            {
                throw new Exception("Can't register new customers with this email! " + e.Message);
            }
        }

        public Response VerifyEmail(string code)
        {
            var response = new Response();
            if (code == "access_denied")
            {
                response.SetCode(CodeTypes.Err_AccessDenied);
                response.SetResult("OAuth authentication access denied!");
                return response;
            }

            var oAuthToken = GetOAuthToken(code);
            var oAuthUserInfo = GetOAuthUserInfo(oAuthToken);

            var customerExist = GetCusByEmail(oAuthUserInfo.Email);
            if (customerExist == null)
            {
                customerExist = Register(oAuthUserInfo);
            }

            if (customerExist.VerifyEmail != "Y")
            {
                response.SetCode(CodeTypes.Err_Exist);
                response.SetResult(string.Format(@"Account with this phone number [{0}] has not been verified!", customerExist.Email));
            }

            response.SetCode(CodeTypes.Success);
            response.SetResult(_customerService.GetLoginResponse(customerExist));
            return response;
        }
    }
}
