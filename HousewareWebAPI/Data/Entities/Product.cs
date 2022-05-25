using HousewareWebAPI.Helpers.Attribute;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Data.Entities
{
    public class Product
    {
        public string ProductId { get; set; }
        [MyRequired]
        public string Name { get; set; }
        public int Sort { get; set; }
        [MyRequired]
        public string Avatar { get; set; }
        /// <summary>
        /// Type: Json String. Property of a item: image
        /// </summary>
        public string Images { get; set; }
        public uint Price { get; set; }
        public int View { get; set; }
        /// <summary>
        /// Type: Json String. Property of a item: highlight
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
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Enable { get; set; }

        // Navigation
        public string CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public ICollection<ProductSpecification> ProductSpecifications { get; set; }
        public ICollection<Stored> Storeds { get; set; }
    }
}
