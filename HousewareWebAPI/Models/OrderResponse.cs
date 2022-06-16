using HousewareWebAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Models
{
    public class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string SortCode { get; set; }
        public StoreResponse Store { get; set; }
        public AddressResponse Address { get; set; }
        public List<ProInCartResponse> Products { get; set; } = new();
        public uint TotalPrice { get; set; }
        public uint TotalFee { get; set; }
        public uint Total { get; set; }
        public DateTime ExpectedDeliveryTime { get; set; }
    }

    public class GetOrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public List<ProInCartResponse> Products { get; set; } = new();
        public uint TotalPrice { get; set; }
        public uint TotalFee { get; set; }
        public uint Total { get; set; }
        public string Status { get; set; }
        public GetOrderResponse(Order order)
        {
            OrderId = order.OrderId;
            OrderCode = order.OrderCode;
            foreach (var orderDetail in order.OrderDetails)
            {
                Products.Add(new ProInCartResponse
                {
                    ProductId = orderDetail.ProductId,
                    Name = orderDetail.Product.Name,
                    Avatar = orderDetail.Product.Avatar,
                    ItemPrice = orderDetail.Product.Price,
                    Quantity = orderDetail.Quantity,
                    Price = orderDetail.Product.Price * orderDetail.Quantity
                });
            }
            TotalPrice = (uint)Products.Sum(p => p.Price);
            Total = order.Amount;
            TotalFee = Total - TotalPrice;
            Status = order.OrderStatus;
        }
    }
}
