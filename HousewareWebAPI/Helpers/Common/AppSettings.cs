namespace HousewareWebAPI.Helpers.Common
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
        public string GHNURLCreateOrder { get; set; }
        public string GHNURLPreviewOrder { get; set; }
        public string GHNURLGetInfo { get; set; }
        public string GHNURLGetService { get; set; }
        // VNP
        public string VNP_TmnCode { get; set; }
        public string VNP_HashSecret { get; set; }
        public string VNP_Url { get; set; }
        // Twilio
        public string TwilioAccountSID { get; set; }
        public string TwilioAuthToken { get; set; }
        public string TwilioPhoneNumber { get; set; }
        public string TwilioServiceSID { get; set; }
        // SendGrid
        public string SendGridKey { get; set; }
    }
}
