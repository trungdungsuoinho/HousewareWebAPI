using Newtonsoft.Json.Linq;

namespace HousewareWebAPI.Models
{
    public class CatInGetAllClass
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
    }

    public class CatInGetClass
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Image { get; set; }
    }

    public class GetCategoryResponse
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        //public string Video { get; set; }
        public JArray Advantage { get; set; }
    }

    public class GetCatAdminResponse
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Image { get; set; }
        //public string Video { get; set; }
        public JArray Advantage { get; set; }
        public bool Enable { get; set; }
        public string ClassificationId { get; set; }
    }
}
