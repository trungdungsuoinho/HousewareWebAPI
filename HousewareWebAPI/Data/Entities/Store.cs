using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HousewareWebAPI.Data.Entities
{
    public class Store
    {
        public int StoreId { get; set; }
        public int ShopId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        public string Phone { get; set; }
        public int Province { get; set; }
        public int District { get; set; }
        [MyRequired]
        public string Ward { get; set; }
        [MyRequired]
        public string Detail { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Stored> Storeds { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
