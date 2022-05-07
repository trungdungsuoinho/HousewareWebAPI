using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
        public ImageInput Image { get; set; }
        //public AddVideoRequest Video { get; set; }
        public List<Advantage> Advantages { get; set; }
        public bool? Enable { get; set; }
        [MyRequired]
        public string ClassificationId { get; set; }
    }

    public class Advantage
    {
        public string Head { get; set; }
        public string Content { get; set; }
    }

    public class ModifySortCatAdminRequest
    {
        [MyRequired]
        public string ClassificationId { get; set; }
        [MyRequired]
        public List<string> CategoryIds { get; set; }
    }
}
