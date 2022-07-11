using HousewareWebAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Models
{
    public class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public StoreResponse Store { get; set; }
        public AddressResponse Address { get; set; }
        public List<ProductInCartResponse> Products { get; set; } = new();
        public int TotalPrice { get; set; }
        public int TotalFee { get; set; }
        public int Total { get; set; }

        public CreateOrderResponse()
        {
        }

        public CreateOrderResponse(Order order)
        {
            OrderId = order.OrderId;
            OrderDate = order.OrderDate;
            foreach (var orderDetail in order.OrderDetails)
            {
                Products.Add(new ProductInCartResponse
                {
                    ProductId = orderDetail.ProductId,
                    Name = orderDetail.Product.Name,
                    Avatar = orderDetail.Product.Avatar,
                    ItemPrice = orderDetail.Product.Price,
                    Quantity = orderDetail.Quantity,
                    Price = orderDetail.Product.Price * orderDetail.Quantity
                });
            }
            TotalPrice = Products.Sum(p => p.Price);
        }
    }

    public class GetOrderPagingResponse
    {
        public List<GetOrderResponse> Orders { get; set; } = new();
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int Size { get; set; }
    }

    public class GetOrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public StoreResponse Store { get; set; }
        public AddressResponse Address { get; set; }
        public List<ProductInCartResponse> Products { get; set; } = new();
        public int TotalPrice { get; set; }
        public int TotalFee { get; set; }
        public int Total { get; set; }
        public string Status { get; set; }
        
        public GetOrderResponse()
        {
        }

        public GetOrderResponse(Order order)
        {
            OrderId = order.OrderId;
            OrderCode = order.OrderCode;
            OrderDate = order.OrderDate;
            foreach (var orderDetail in order.OrderDetails)
            {
                Products.Add(new ProductInCartResponse
                {
                    ProductId = orderDetail.ProductId,
                    Name = orderDetail.Product.Name,
                    Avatar = orderDetail.Product.Avatar,
                    ItemPrice = orderDetail.Product.Price,
                    Quantity = orderDetail.Quantity,
                    Price = orderDetail.Product.Price * orderDetail.Quantity
                });
            }
            TotalPrice = Products.Sum(p => p.Price);
            TotalFee = order.Fee;
            Total = TotalPrice + TotalFee;
            Status = order.OrderStatus;
        }
    }
}
