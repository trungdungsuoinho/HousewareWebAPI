namespace HousewareWebAPI.Helpers.Common
{
    public class AppSettings
    {
        public string DBConnectionString { get; set; }
        public string ImageCloudName { get; set; }
        public string ImageAPIKey { get; set; }
        public string ImageAPISecret { get; set; }
        public bool UsingJWT { get; set; }
        public string Secret { get; set; }
    }
}
