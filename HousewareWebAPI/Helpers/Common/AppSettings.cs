﻿namespace HousewareWebAPI.Helpers.Common
{
    public class AppSettings
    {
        public string DBConnectionString { get; set; }
        public string ImageCloudName { get; set; }
        public string ImageAPIKey { get; set; }
        public string ImageAPISecret { get; set; }
        public string OAuthClientId { get; set; }
        public string OAuthClientSeret { get; set; }
        public string OAuthRedirectURI { get; set; }
        public bool UsingJWT { get; set; }
        public string Secret { get; set; }
        // GHN
        public int GHNClientId { get; set; }
        public int GHNShopId { get; set; }
        public string GHNToken { get; set; }
        public string GHNURLGetInfo { get; set; }


        public string VNP_TmnCode { get; set; }
        public string VNP_HashSecret { get; set; }
        public string VNP_Url { get; set; }
    }
}
