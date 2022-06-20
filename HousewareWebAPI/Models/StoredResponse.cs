using HousewareWebAPI.Data.Entities;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class ProGetStoredResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public ProGetStoredResponse(Stored stored)
        {
            ProductId = stored.ProductId;
            Name = stored.Product?.Name;
            Quantity = stored.Quantity;
        }
    }

    public class ProChangeStoredReponse : ProGetStoredResponse
    {
        public ProChangeStoredReponse(Stored store, uint importQuantity) : base(store)
        {
            ChangeQuantity = importQuantity;
        }

        public uint ChangeQuantity { get; set; }
    }

    public class GetStoredResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public List<ProChangeStoredReponse> Products { get; set; } = new();
        public GetStoredResponse(Store store)
        {
            StoreId = store.StoreId;
            Name = store.Name;
            if (store.Storeds != null && store.Storeds.Count > 0)
            {
                foreach (var stored in store.Storeds)
                {
                    Products.Add(new ProChangeStoredReponse(stored));
                }
            }
        }
    }
}
