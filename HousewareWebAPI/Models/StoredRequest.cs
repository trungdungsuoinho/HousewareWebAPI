using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class GetStoredRequest
    {
        [MyRequired]
        public int StoreId { get; set; }
    }

    public class ProStoredRequest
    {
        [MyRequired]
        public string ProductId { get; set; }
    }

    public class StoredRequest
    {
        [MyRequired]
        public int StoreId { get; set; }
        public List<ProStoredRequest> Products { get; set; } = new();
    }

    public class ProChangeStoredRequest : ProStoredRequest
    {
        [MyRequired]
        [MyRange(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class ChangeStoredRequest
    {
        [MyRequired]
        public int StoreId { get; set; }
        public List<ProChangeStoredRequest> Products { get; set; } = new();
    }
}
