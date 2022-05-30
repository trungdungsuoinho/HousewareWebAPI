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
        public int Province { get; set; }
        public int District { get; set; }
        [MyRequired]
        public string Ward { get; set; }
        [MyRequired]
        public string Detail { get; set; }
    }

    public class UpdateStoreRequest : AddStoreRequest
    {
        public int StoreId { get; set; }
    }
}
