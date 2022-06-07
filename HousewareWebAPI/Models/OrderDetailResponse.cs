namespace HousewareWebAPI.Models
{
    public class OrderDetailResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public uint Price { get; set; }
        public uint Quantity { get; set; }
        public uint ItemPrice { get; set; }
    }
}
