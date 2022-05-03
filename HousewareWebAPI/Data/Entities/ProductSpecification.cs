using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual Product Product { get; set; }
        public virtual Specification Specification { get; set; }
    }
}
