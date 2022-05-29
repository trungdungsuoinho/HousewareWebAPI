using HousewareWebAPI.Helpers.Attribute;

namespace HousewareWebAPI.Models
{
    public class StoredRequest
    {
        [MyRequired]
        public int StoreId { get; set; }
        [MyRequired]
        public string ProductId { get; set; }
        [MyRequired]
        [MyRange(1, int.MaxValue)]
        public uint Quantity { get; set; }
    }
}
