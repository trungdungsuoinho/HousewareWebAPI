using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    /// <summary>
    /// View get all classification for Client
    /// </summary>
    public class GetAllClassificationResponse
    {
        public string ClassificationId { get; set; }
        public string Name { get; set; }
        public string ImageMenu { get; set; }
        public List<CatInGetAllClass> Categories { get; set; }
    }

    /// <summary>
    /// View get a classification for Client
    /// </summary>
    public class GetClassificationResponse
    {
        public string ClassificationId { get; set; }
        public string Name { get; set; }
        public string ImageBanner { get; set; }
        //public JArray Story { get; set; } // Currently not implemented
        public List<CatInGetClass> Categories { get; set; }
    }

    /// <summary>
    /// View get classification for Client
    /// </summary>
    public class GetClassAdminResponse
    {
        public string ClassificationId { get; set; }
        public string Name { get; set; }
        public string ImageMenu { get; set; }
        public string ImageBanner { get; set; }
        //public JArray Story { get; set; } // Currently not implemented
        public bool Enable { get; set; }
    }
}
