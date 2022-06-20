using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class ProInStoredRequest
    {
        [MyRequired]
        public string ProductId { get; set; }
        [MyRequired]
        [MyRange(1, int.MaxValue)]
        public uint Quantity { get; set; }
    }
    public class StoredRequest
    {
        [MyRequired]
        public int StoreId { get; set; }
        public List<ProInStoredRequest> Products { get; set; } = new();
    }
}
