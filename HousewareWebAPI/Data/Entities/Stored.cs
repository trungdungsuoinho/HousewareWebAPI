using Newtonsoft.Json;

namespace HousewareWebAPI.Data.Entities
{
    public class Stored
    {
        public int StoreId { get; set; }
        public string ProductId { get; set; }
        public uint Quantity { get; set; }

        // Navigation
        [JsonIgnore]
        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
