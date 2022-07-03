using HousewareWebAPI.Data.Entities;
using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class ProductSummaryResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Price { get; set; }
        //public int Review { get; set; }
        //public float Rate { get; set; }
        public ProductSummaryResponse(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            Avatar = product.Avatar;
            Price = product.Price;
        }
    }

    public class ProductResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public List<string> Images { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int View { get; set; }
        public List<string> Highlights { get; set; }
        public List<GetSpecByPro> Specifications { get; set; }
    }

    public class ProductAdminResponse
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<string> Images { get; set; }
        public int Price { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int View { get; set; }
        public List<string> Highlights { get; set; }
        //public string Overview { get; set; }
        //public string Design { get; set; }
        //public string Performance { get; set; }
        public List<GetSpecByPro> Specifications { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Enable { get; set; }
    }

    public class ProductSearchResponse
    {
        public ProductSummaryResponse Product { get; set; }
        public float Score { get; set; }
    }
}
