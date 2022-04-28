using HousewareWebAPI.Helpers.Attribute;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Houseware.WebAPI.Entities
{
    public class Category
    {
        public string CategoryId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public int? Sort { get; set; }
        public string Slogan { get; set; }
        [MyRequired]
        public string Image { get; set; }
        public string Video { get; set; }
        /// <summary>
        /// Type: JArray String. Property of a item: head, content
        /// </summary>
        public string Advantage { get; set; }
        public bool Enable { get; set; }

        // Navigation
        public string ClassificationId { get; set; }
        [JsonIgnore]
        public virtual Classification Classification { get; set; }
    }
}
