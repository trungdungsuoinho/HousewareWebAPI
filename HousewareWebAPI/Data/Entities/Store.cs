using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HousewareWebAPI.Data.Entities
{
    public class Store
    {
        public int StoreId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Province { get; set; }
        [MyRequired]
        public string District { get; set; }
        [MyRequired]
        public string Ward { get; set; }
        [MyRequired]
        public string Detail { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Stored> Storeds { get; set; }
    }
}
