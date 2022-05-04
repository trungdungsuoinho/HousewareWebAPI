using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json.Linq;

namespace HousewareWebAPI.Models
{
    public class AddClassAdminRequest
    {
        [MyRequired]
        public string ClassificationId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        public AddImageRequest ImageMenu { get; set; }
        public AddImageRequest ImageBanner { get; set; }
        public JArray Story { get; set; }
        public bool? Enable { get; set; }
    }
}
