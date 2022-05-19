namespace HousewareWebAPI.Helpers.Models
{
    public class OAuthToken
    {
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Scope { get; set; }
        public string Token_type { get; set; }
    }

    public class OAuthUserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool Verified_email { get; set; }
        public string Name { get; set; }
        public string Given_name { get; set; }
        public string Picture { get; set; }
        public string Locale { get; set; }
    }
}
