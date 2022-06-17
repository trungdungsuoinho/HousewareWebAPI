using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface IOrderService
    {
        public Response CreateOrderOffline(CreateOrderOffineRequest model);
        public Response CreateOrderOnline(CreateOrderOnlineRequest model);
        public IPNVNPayResponse IPNVNPay(CodeIPNURLRequest model);
        public Response GetResutlOrderOnline(GetPreviewOrderRequest model);
        public Response GetOrders(GetOrdersRequest model);
    }

    public class OrderService : IOrderService
    {
        private readonly HousewareContext _context;
        private readonly IGHNService _gHNService;
        private readonly IVNPayService _vNPayService;

        public OrderService(HousewareContext context, IGHNService gHNService, IVNPayService vNPayService)
        {
            _context = context;
            _gHNService = gHNService;
            _vNPayService = vNPayService;
        }

        private List<OrderDetail> GetOrderDetails(Guid orderId)
        {
            return _context.OrderDetails.Where(o => o.OrderId == orderId).Include(o => o.Product).ToList();
        }

        public Response CreateOrderOffline(CreateOrderOffineRequest model)
        {
            Response response = new();
            try
            {
                GHNCreateOrderRequest createOrderRequest = new();
                var customer = _context.Customers.Where(c => c.CustomerId == model.CustomerId).FirstOrDefault();
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
                    order.Amount = (uint)(createOrderRequest.Insurance_value + jObjCreateOrder.Value<int>("total_fee"));
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
                        Total = order.Amount,
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

        public Response CreateOrderOnline(CreateOrderOnlineRequest model)
        {
            Response response = new();
            try
            {
                GHNCreateOrderRequest createOrderRequest = new();
                var customer = _context.Customers.Where(c => c.CustomerId == model.CustomerId).FirstOrDefault();
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

                // Create priview order
                Order order = new();
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    order.CustomerId = model.CustomerId;
                    order.AddressId = model.AddressId;
                    order.StoreId = model.StoreId;
                    order.PaymentType = GlobalVariable.PayOnline;
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

                    var jObjCreateOrder = _gHNService.PreviewOrder(createOrderRequest, store.ShopId);

                    order.Amount = (uint)(createOrderRequest.Insurance_value + jObjCreateOrder.Value<int>("total_fee"));
                    order.OrderStatus = GlobalVariable.OrderPaymenting;
                    _context.Entry(order).State = EntityState.Modified;
                    _context.SaveChanges();

                    var url = _vNPayService.GeneratePaymentURL(model.ReturnUrl, order.OrderId, (int)order.Amount);

                    transaction.Commit();
                    response.SetCode(CodeTypes.Success);
                    response.SetResult(url);
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

        public IPNVNPayResponse IPNVNPay(CodeIPNURLRequest model)
        {
            IPNVNPayResponse response = new();
            bool checkSignature = _vNPayService.ValidateSignature(model);
            if (checkSignature)
            {
                var order = _context.Orders.Where(o => o.OrderId.ToString() == model.Vnp_TxnRef).FirstOrDefault();
                if (order != null)
                {
                    if (order.Amount == model.Vnp_Amount/100)
                    {
                        if (model.Vnp_ResponseCode == "00" && model.Vnp_TransactionStatus == "00")
                        {
                            order.OrderStatus = GlobalVariable.OrderPaymented;
                            order.TransactionNo = model.Vnp_TransactionNo;
                            _context.SaveChanges();

                            // Create order GHN
                            GHNCreateOrderRequest createOrderRequest = new();
                            var customer = _context.Customers.Where(c => c.CustomerId == order.CustomerId).FirstOrDefault();
                            if (customer == null)
                            {
                                throw new Exception("Customer does not exist!");
                            }
                            createOrderRequest.To_name = customer.FullName ?? GlobalVariable.GHNToNameDefault;

                            _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();
                            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                            {
                                throw new Exception("This cart is empty!");
                            }
                            createOrderRequest.SetValueProduct(order.OrderDetails.ToList());

                            var address = _context.Addresses.Where(a => a.AddressId == order.AddressId).FirstOrDefault();
                            if (address == null)
                            {
                                throw new Exception("This address could not be found in the customer's address book!");
                            }
                            createOrderRequest.SetValueAddress(address);

                            var store = _context.Stores.Where(s => s.StoreId == order.StoreId).FirstOrDefault();
                            if (store == null)
                            {
                                throw new Exception("Store does not exist!");
                            }
                            createOrderRequest.SetValueStore(store);
                            createOrderRequest.Client_order_code = order.OrderId.ToString();
                            createOrderRequest.Cod_amount = 0;
                            createOrderRequest.Content = GlobalVariable.GHNContent(createOrderRequest.Client_order_code);
                            createOrderRequest.Payment_type_id = 1;
                            createOrderRequest.Note = order.Note;

                            var jObjCreateOrder = _gHNService.CreateOrder(createOrderRequest, store.ShopId);

                            order.OrderCode = jObjCreateOrder.Value<string>("order_code");
                            order.OrderStatus = GlobalVariable.OrderDoing;
                            _context.Entry(order).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                        response.RspCode = "00";
                        response.Message = "Confirm Success";
                        order.Note = "00";
                    }
                    else
                    {
                        response.RspCode = "04";
                        response.Message = "Invalid amount";
                        order.Note = "04";
                    }
                }
                else
                {
                    response.RspCode = "01";
                    response.Message = "Order not found";
                    order.Note = "01";
                }
            }
            else
            {
                response.RspCode = "97";
                response.Message = "Invalid signature";
            }
            return response;
        }

        public Response GetResutlOrderOnline(GetPreviewOrderRequest model)
        {
            Response response = new();
            try
            {
                var order = _context.Orders.Where(o => o.OrderId == model.OrderId).FirstOrDefault();
                if (order == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Order does not exist!");
                    return response;
                }

                _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();
                if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This order is empty!");
                    return response;
                }

                var address = _context.Addresses.Where(a => a.AddressId == order.AddressId).FirstOrDefault();
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address could not be found in the customer's address book!");
                    return response;
                }

                var store = _context.Stores.Where(s => s.StoreId == order.StoreId).FirstOrDefault();
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Store does not exist!");
                    return response;
                }

                // Create response
                JObject jObjGetOrder = new();
                try
                {
                    jObjGetOrder = _gHNService.GetOrder(order.OrderId.ToString());
                }
                catch (Exception e)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Order GHN does not exist" + e.Message);
                    return response;
                }

                CreateOrderResponse orderResponse = new()
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    SortCode = jObjGetOrder.Value<string>("sort_code"),
                    Store = new StoreResponse(store),
                    Address = new AddressResponse(address),
                    TotalPrice = (uint)jObjGetOrder.Value<int>("insurance_value"),
                    TotalFee = order.Amount - (uint)jObjGetOrder.Value<int>("insurance_value"),
                    Total = order.Amount,
                    ExpectedDeliveryTime = jObjGetOrder.Value<DateTime>("leadtime")
                };
                foreach (var orderDetail in order.OrderDetails)
                {
                    orderResponse.Products.Add(new ProInCartResponse
                    {
                        ProductId = orderDetail.ProductId,
                        Name = orderDetail.Product.Name,
                        Avatar = orderDetail.Product.Avatar,
                        Price = orderDetail.Product.Price,
                        Quantity = orderDetail.Quantity,
                        ItemPrice = orderDetail.Product.Price * orderDetail.Quantity
                    });
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(orderResponse);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response GetOrders(GetOrdersRequest model)
        {
            Response response = new();
            try
            {
                var customer = _context.Customers.Where(o => o.CustomerId == model.CustomerId).Include(c => c.Orders).FirstOrDefault();
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Customer does not exist!");
                    return response;
                }

                List<GetOrderResponse> orderResponses = new();
                if (customer.Orders != null && customer.Orders.Count > model.Step * GlobalVariable.StepWidth)
                {
                    for (int i = (int)(model.Step* GlobalVariable.StepWidth);
                        i < customer.Orders.Count && i < model.Step * GlobalVariable.StepWidth + GlobalVariable.StepWidth;
                        i++)
                    {
                        var order = customer.Orders.ToList()[i];
                        _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();
                        var orderRes = new GetOrderResponse(order);
                        if (order.OrderStatus == GlobalVariable.OrderDoing)
                        {
                            var orderInfo = _gHNService.OrderInfo(new GHNOrderInfoRequest(order.OrderCode));
                            if (orderInfo.Code == 200)
                            {
                                var status = orderInfo.Data.Value<string>("status");
                                if (status == "cancel")
                                {
                                    order.OrderStatus = GlobalVariable.OrderCancel;
                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                    orderRes.Status = GlobalVariable.OrderCancel;
                                }
                                if (status == "delivered")
                                {
                                    order.OrderStatus = GlobalVariable.OrderDone;
                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                    orderRes.Status = GlobalVariable.OrderDone;
                                }
                            }
                        }
                        orderResponses.Add(orderRes);
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(orderResponses);
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
