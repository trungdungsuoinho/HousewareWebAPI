using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IOrderService
    {
        public Response CreateOrder(CreateOrderRequest model);
    }

    public class OrderService : IOrderService
    {
        private readonly HousewareContext _context;
        private readonly IGHNService _gHNService;

        public OrderService(HousewareContext context, IGHNService gHNService)
        {
            _context = context;
            _gHNService = gHNService;
        }

        private List<OrderDetail> GetOrderDetails(Guid orderId)
        {
            return _context.OrderDetails.Where(o => o.OrderId == orderId).Include(o => o.Product).ToList();
        }

        public Response CreateOrder(CreateOrderRequest model)
        {
            Response response = new();
            try
            {
                GHNCreateOrderRequest createOrderRequest = new();
                var customer = _context.Customers.Where(c => c.CustomerId == model.CustomerId).FirstOrDefault(); //.Include(cu => cu.Carts.Select(ca => ca.Product)).FirstOrDefault();
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Customer does not exist!");
                    return response;
                }
                createOrderRequest.To_name = customer.FullName ?? GlobalVariable.GHNToNameDefault;

                _context.Entry(customer).Collection(cu => cu.Carts).Query().Include(ca => ca.Product).Load();
                if (customer.Carts == null || customer.Carts.Count == 0)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This cart is empty!");
                    return response;
                }
                createOrderRequest.SetValueProduct(customer.Carts.ToList());

                var address = _context.Addresses.Where(a => a.AddressId == model.AddressId && a.CustomerId == model.CustomerId).FirstOrDefault();
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address could not be found in the customer's address book!");
                    return response;
                }
                createOrderRequest.SetValueAddress(address);

                var store = _context.Stores.Where(s => s.StoreId == model.StoreId).FirstOrDefault();
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Store does not exist!");
                    return response;
                }
                createOrderRequest.SetValueStore(store);

                // Create order and move list product from cart into order detail
                Order order = new();
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    order.CustomerId = model.CustomerId;
                    order.AddressId = model.AddressId;
                    order.StoreId = model.StoreId;
                    order.PaymentType = GlobalVariable.PayCod;
                    order.Note = model.Note;
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var cart in customer.Carts)
                    {
                        OrderDetail orderDetail = new()
                        {
                            OrderId = order.OrderId,
                            ProductId = cart.ProductId,
                            Quantity = cart.Quantity,
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }
                    _context.SaveChanges();

                    _context.Carts.RemoveRange(customer.Carts);
                    _context.SaveChanges();

                    // Create order GHN
                    createOrderRequest.Client_order_code = order.OrderId.ToString();
                    createOrderRequest.Cod_amount = createOrderRequest.Insurance_value;
                    createOrderRequest.Content = GlobalVariable.GHNContent(createOrderRequest.Client_order_code);
                    createOrderRequest.Payment_type_id = 2;
                    createOrderRequest.Note = order.Note;

                    var jObjCreateOrder = _gHNService.CreateOrder(createOrderRequest, store.ShopId);

                    order.OrderCode = jObjCreateOrder.Value<string>("order_code");
                    order.OrderStatus = GlobalVariable.OrderDoing;
                    _context.Entry(order).State = EntityState.Modified;
                    _context.SaveChanges();                    

                    // Create response
                    CreateOrderResponse orderResponse = new()
                    {
                        OrderId = order.OrderId,
                        OrderCode = order.OrderCode,
                        SortCode = jObjCreateOrder.Value<string>("sort_code"),
                        Store = new StoreResponse(store),
                        Address = new AddressResponse(address),
                        TotalPrice = (uint)createOrderRequest.Insurance_value,
                        TotalFee = (uint)jObjCreateOrder.Value<int>("total_fee"),
                        Total = (uint)(createOrderRequest.Insurance_value + jObjCreateOrder.Value<int>("total_fee")),
                        ExpectedDeliveryTime = jObjCreateOrder.Value<DateTime>("expected_delivery_time")
                    };
                    var orderDetails = _context.OrderDetails.Where(o => o.OrderId == order.OrderId).Include(o => o.Product).ToList();
                    foreach (var orderDetail in orderDetails)
                    {
                        orderResponse.Products.Add(new ProInCartResponse
                        {
                            ProductId = orderDetail.ProductId,
                            Name = orderDetail.Product.Name,
                            Avatar = orderDetail.Product.Avatar,
                            Price = orderDetail.Product.Price,
                            Quantity = orderDetail.Quantity,
                            ItemPrice = orderDetail.Product.Price* orderDetail.Quantity
                        });
                    }
                    transaction.Commit();
                    response.SetCode(CodeTypes.Success);
                    response.SetResult(orderResponse);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    response.SetCode(CodeTypes.Err_AccFail);
                    response.SetResult("Create order failed! " + e.Message);
                    return response;
                }
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }


    }
}
