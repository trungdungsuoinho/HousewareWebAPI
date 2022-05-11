using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class AddProAdminRequest
    {
        [MyRequired]
        public string ProductId { get; set; }
        [MyRequired]
        public string CategoryId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        public ImageInput Avatar { get; set; }
        public List<ImageInput> Images { get; set; }
        public int Price { get; set; }
        public List<string> Highlights { get; set; }
        public List<GetSpecByPro> Specifications { get; set; }
        public bool? Enable { get; set; }
    }

    public class ModifySortProAdminRequest
    {
        [MyRequired]
        public string CategoryId { get; set; }
        [MyRequired]
        public List<string> ProductIds { get; set; }
    }
}
