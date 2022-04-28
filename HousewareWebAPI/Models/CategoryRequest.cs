using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json.Linq;

namespace HousewareWebAPI.Models
{
    public class AddCatAdminRequest
    {
        [MyRequired]
        public string CategoryId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Slogan { get; set; }
        [MyRequired]
        public AddImageRequest Image { get; set; }
        //public AddVideoRequest Video { get; set; }
        public JArray Advantage { get; set; }
        public bool Enable { get; set; }
        public string ClassificationId { get; set; }
    }
}
