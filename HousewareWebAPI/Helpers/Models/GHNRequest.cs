namespace HousewareWebAPI.Helpers.Models
{
    public class GHNRegisterShopRequest
    {
        public int District_id { get; set; }
        public string Ward_code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class GHNCalculateFeeRequest
    {
        public int Service_type_id { get; set; } = 2;
        public int Insurance_value { get; set; }
        public int From_district_id { get; set; }
        public string To_ward_code { get; set; }
        public int To_district_id { get; set; }
        public int Weight { get; set; }
        //public int Length { get; set; }
        //public int Width { get; set; }
        //public int Height { get; set; }

    }
}
