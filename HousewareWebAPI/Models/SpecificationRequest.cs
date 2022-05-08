using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class AddSpecAdminRequest
    {
        [MyRequired]
        public string SpecificationId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ModifySortSpecAdminRequest
    {
        [MyRequired]
        public List<string> SpecificationIds { get; set; }
    }
}
