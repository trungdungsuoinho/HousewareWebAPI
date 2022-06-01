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
        public int ProvinceId { get; set; }
        [MyRequired]
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        [MyRequired]
        public string DistrictName { get; set; }
        [MyRequired]
        public string WardId { get; set; }
        [MyRequired]
        public string WardName { get; set; }
        [MyRequired]
        public string Detail { get; set; }

        // Navigation
        [JsonIgnore]
        public ICollection<Stored> Storeds { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}
