using HousewareWebAPI.Helpers.Attribute;

namespace HousewareWebAPI.Models
{
    public class ImageInput
    {
        [MyRequired]
        public string Content { get; set; }
        public bool IsUrl { get; set; } = false;
        public string Name { get; set; }
    }
}
