using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class AddClassAdminRequest
    {
        [MyRequired]
        public string ClassificationId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        public ImageInput ImageMenu { get; set; }
        public ImageInput ImageBanner { get; set; }
        public JArray Story { get; set; }
        public bool? Enable { get; set; }
    }

    public class ModifySortClassAdminRequest
    {
        [MyRequired]
        public List<string> ClassificationIds { get; set; }
    }
}
