using System.Text.Json.Serialization;

namespace HousewareWebAPI.Data.Entities
{
    public class ProductSpecification
    {
        public string ProductId { get; set; }
        public string SpecificationId { get; set; }
        public string Value { get; set; }

        // Navigation
        [JsonIgnore]
        public Product Product { get; set; }
        public Specification Specification { get; set; }
    }
}
