using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HousewareWebAPI.Data.Entities
{
    public class Specification
    {
        public string SpecificationId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Description { get; set; }

        // Navigation
        [JsonIgnore]
        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }
}
