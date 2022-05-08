namespace HousewareWebAPI.Models
{
    public class GetSpecAdminResponse
    {
        public string SpecificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GetSpecByPro
    {
        public string SpecificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
