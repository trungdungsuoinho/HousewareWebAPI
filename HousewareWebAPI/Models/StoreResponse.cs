using HousewareWebAPI.Data.Entities;

namespace HousewareWebAPI.Models
{
    public class StoreResponse
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string Detail { get; set; }
        public StoreResponse(Store model)
        {
            StoreId = model.StoreId;
            Name = model.Name;
            ProvinceId = model.ProvinceId;
            ProvinceName = model.ProvinceName;
            DistrictId = model.DistrictId;
            DistrictName = model.DistrictName;
            WardId = model.WardId;
            WardName = model.WardName;
            Detail = model.Detail;
        }
    }

    public class GetCalculateFee
    {
        public StoreResponse Store { get; set; }
        public uint Fee { get; set; }
    }
}
