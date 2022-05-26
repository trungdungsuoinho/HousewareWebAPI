using HousewareWebAPI.Helpers.Attribute;

namespace HousewareWebAPI.Models
{
    public class AddStoreRequest
    {
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        public string Province { get; set; }
        [MyRequired]
        public string District { get; set; }
        [MyRequired]
        public string Ward { get; set; }
        [MyRequired]
        public string Detail { get; set; }
    }

    public class UpdateStoreRequest : AddStoreRequest
    {
        [MyRequired]
        public int StoreId { get; set; }
    }
}
