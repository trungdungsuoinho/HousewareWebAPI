using HousewareWebAPI.Data.Entities;

namespace HousewareWebAPI.Models
{
    public class GetStoreResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public int Province { get; set; }
        public int District { get; set; }
        public string Ward { get; set; }
        public string Detail { get; set; }
        public GetStoreResponse(Store model)
        {
            StoreId = model.StoreId;
            Name = model.Name;
            Province = model.Province;
            District = model.District;
            Ward = model.Ward;
            Detail = model.Detail;
        }
    }

    public class GetStoresResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public GetStoresResponse(Store store)
        {
            StoreId = store.StoreId;
            Name = store.Name;
            FullAddress = string.Format(@"{0}, {1}, {2}, {3}", store.Detail, store.Ward, store.District, store.Province);
        }

        public void SetValue(Store store)
        {
            StoreId = store.StoreId;
            Name = store.Name;
            FullAddress = string.Format(@"{0}, {1}, {2}, {3}", store.Detail, store.Ward, store.District, store.Province);
        }
    }
}
