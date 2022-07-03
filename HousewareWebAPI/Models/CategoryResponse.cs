using System.Collections.Generic;

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
        public List<Advantage> Advantages { get; set; }
        public List<ProductSummaryResponse> Products { get; set; }
    }

    public class GetCatAdminResponse
    {
        public string CategoryId { get; set; }
        public string ClassificationId { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Image { get; set; }
        //public string Video { get; set; }
        public List<Advantage> Advantages { get; set; }
        public bool Enable { get; set; }
    }
}
