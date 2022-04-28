using HousewareWebAPI.Helpers.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Data.Entities
{
    public class Product
    {
        public string ProductId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        [MyRequired]
        public string Avatar { get; set; }
        /// <summary>
        /// Type: JArray String. Property of a item: image
        /// </summary>
        public string Images { get; set; }
        public int Price { get; set; }
        public int View { get; set; }
        /// <summary>
        /// Type: JArray String. Property of a item: image
        /// </summary>
        public string Highlights { get; set; }
        /// <summary>
        /// JsonString
        /// </summary>
        public string Overview { get; set; }
        /// <summary>
        /// JsonString
        /// </summary>
        public string Design { get; set; }
        /// <summary>
        /// JsonString
        /// </summary>
        public string Performance { get; set; }
        public bool Enable { get; set; }
    }
}
