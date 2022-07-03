using HousewareWebAPI.Data.Entities;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class ProGetStoredResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public ProGetStoredResponse(Stored stored)
        {
            ProductId = stored.ProductId;
            Name = stored.Product?.Name;
            Quantity = stored.Quantity;
        }
    }

    public class GetStoredResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public List<ProGetStoredResponse> Products { get; set; } = new();
        public GetStoredResponse(Store store)
        {
            StoreId = store.StoreId;
            Name = store.Name;
        }
    }

    public class ProStoredResponse : ProGetStoredResponse
    {
        public ProStoredResponse(Stored store, int importQuantity) : base(store)
        {
            ChangeQuantity = importQuantity;
        }

        public int ChangeQuantity { get; set; }
    }

    public class GetChangeStoredResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public List<ProStoredResponse> Products { get; set; } = new();
        public GetChangeStoredResponse(Store store)
        {
            StoreId = store.StoreId;
            Name = store.Name;
        }
    }
}
