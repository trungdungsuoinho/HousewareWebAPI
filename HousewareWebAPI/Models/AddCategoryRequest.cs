using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Models
{
    public class AddCategoryRequest
    {
        [MyRequired]
        public string CategoryId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public string Slogan { get; set; }
        [MyRequired]
        public AddImageRequest Image { get; set; }
        public JArray Advantage { get; set; }
        public bool Enable { get; set; }
    }
}
