using HousewareWebAPI.Helpers.Attribute;

namespace HousewareWebAPI.Models
{
    public class LoginRequest
    {
        [MyRequired]
        [MyRegularExpression(@"^([0-9]{10})$")]
        public string Phone { get; set; }
        [MyRequired]
        public string Password { get; set; }
    }
}
