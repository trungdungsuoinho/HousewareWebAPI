using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class AddProductRequest
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
        [MyRange(0, int.MaxValue)]
        public int Price { get; set; }
        [MyRange(0, int.MaxValue)]
        public int Weight { get; set; }
        [MyRange(0, int.MaxValue)]
        public int Length { get; set; }
        [MyRange(0, int.MaxValue)]
        public int Width { get; set; }
        [MyRange(0, int.MaxValue)]
        public int Height { get; set; }
        public List<string> Highlights { get; set; }
        public List<AddValueSpec> Specifications { get; set; }
        public bool Enable { get; set; } = false;
    }

    public class ModifySortProductRequest
    {
        [MyRequired]
        public string CategoryId { get; set; }
        [MyRequired]
        public List<string> ProductIds { get; set; }
    }

    public class SearchProductRequest
    {
        [MyRequired]
        public string Content { get; set; }
    }
}
