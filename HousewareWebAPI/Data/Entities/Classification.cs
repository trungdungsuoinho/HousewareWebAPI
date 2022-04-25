using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;

namespace Houseware.WebAPI.Entities
{
    public class Classification
    {
        public string ClassificationId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public int? Sort { get; set; }
        [MyRequired]
        public string ImageMenu { get; set; }
        public string ImageBanner { get; set; }
        /// <summary>
        /// Type: JArray String. Property of a item: content
        /// </summary>
        public string Story { get; set; }
        public bool Enable { get; set; }

        // Navigation
        public virtual ICollection<Category> Categories { get; set; }
    }
}
