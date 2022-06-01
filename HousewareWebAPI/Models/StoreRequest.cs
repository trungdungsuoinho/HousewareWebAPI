using HousewareWebAPI.Helpers.Attribute;

namespace HousewareWebAPI.Models
{
    public class AddStoreRequest
    {
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        [MyRegularExpression(@"^([0-9]{10})$")]
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        [MyRequired]
        public string ProvinceName { get; set; }
        public int DistrictId { get; set; }
        [MyRequired]
        public string DistrictName { get; set; }
        [MyRequired]
        [MyRegularExpression(@"^[0-9]*$")]
        public string WardId { get; set; }
        [MyRequired]
        public string WardName { get; set; }
        [MyRequired]
        public string Detail { get; set; }
    }

    public class UpdateStoreRequest : AddStoreRequest
    {
        public int StoreId { get; set; }
    }
}
