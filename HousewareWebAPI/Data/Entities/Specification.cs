using HousewareWebAPI.Helpers.Attribute;

namespace HousewareWebAPI.Data.Entities
{
    public class Specification
    {
        public string SpecificationId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
