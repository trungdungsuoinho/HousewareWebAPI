using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Models
{
    public class GetClassificationResponse
    {
        public string ClassificationId { get; set; }
        public string Name { get; set; }
        public string ImageMenu { get; set; }
        public string ImageBanner { get; set; }
    }
}
