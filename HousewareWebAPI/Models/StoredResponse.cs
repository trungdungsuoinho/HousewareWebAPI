namespace HousewareWebAPI.Models
{
    public class GetStoredResponse
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set;}
        public uint Quantity { get; set; }
    }

    public class ImportStoredResponse : GetStoredResponse
    {
        public uint ImportQuantity { get; set; }
        public ImportStoredResponse(GetStoredResponse stored)
        {
            StoreId = stored.StoreId;
            StoreName = stored.StoreName;
            ProductId = stored.ProductId;
            ProductName = stored.ProductName;
            Quantity = stored.Quantity;
        }
    }

    public class ExportStoredResponse : GetStoredResponse
    {
        public uint ExportQuantity { get; set; }
        public ExportStoredResponse(GetStoredResponse stored)
        {
            StoreId = stored.StoreId;
            StoreName = stored.StoreName;
            ProductId = stored.ProductId;
            ProductName = stored.ProductName;
            Quantity = stored.Quantity;
        }
    }
}
