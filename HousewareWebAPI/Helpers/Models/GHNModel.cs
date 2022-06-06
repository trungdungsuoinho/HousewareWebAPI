using HousewareWebAPI.Data.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Helpers.Models
{
    public class GHNResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public JObject Data { get; set; }
    }

    public class GHNRegisterShopRequest
    {
        public int District_id { get; set; }
        public string Ward_code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class GHNCalculateFeeRequest
    {
        public int Service_type_id { get; set; } = 2;
        public int Insurance_value { get; set; }
        public int From_district_id { get; set; }
        public string To_ward_code { get; set; }
        public int To_district_id { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public void CalculateProduct(List<Cart> carts)
        {
            double volume = 0;
            foreach (var cart in carts)
            {
                if (cart.Product == null)
                {
                    throw new Exception("There is a product not found in the cart");
                }
                Insurance_value += (int)(cart.Product.Price * cart.Quantity);
                Weight += (int)(cart.Product.Weight * cart.Quantity);
                volume += cart.Product.Length * cart.Product.Width * cart.Product.Height * cart.Quantity;
            }
            volume = Math.Pow(volume, (double)(1 / 3));
            Length = Width = Height = (int)Math.Floor(volume);
        }
    }

    public class GHNCreateOrder
    {
        public string To_name { get; set; }
        public string To_phone { get; set; }
        public string To_address { get; set; }
        public string To_ward_code { get; set; }
        public int To_district_id { get; set; }
        public string Return_phone { get; set; }
        public string Return_address { get; set; }
        public int Return_district_id { get; set; }
        public string Return_ward_code { get; set; }
        public string Client_order_code { get; set; }
        public int Cod_amount { get; set; }
        public string Content { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pick_station_id { get; set; }
        public int Insurance_value { get; set; }
        public int Service_type_id { get; set; } = 2;
        public int Payment_type_id { get; set; }
        public string Note { get; set; }
        public string Required_note { get; set; }
        public List<int> Pick_shift { get; set; } = new List<int> { 1 };
        public List<GHNItem> Items { get; set; } 
    }

    public class GHNItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<JObject> Category { get; set; }
    }
}
