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
        public Response CreateOrder(CreateOrderRequest model);
        public Response GetResutlOrder(OrderIdRequest model);
        public Response CreateOrderOffline(CreateOrderRequest model);
        public Response CreateOrderOnline(CreateOrderOnlineRequest model);
        public IPNVNPayResponse IPNVNPay(CodeIPNURLRequest model);
        public Response GetOrders(GetOrdersRequest model);
        public Response GetOrdersAdmin(GetOrdersAdminRequest model);
        public Response CreateOrderGHN(OrderIdRequest model);
    }

    public class OrderService : IOrderService
    {
        private readonly HousewareContext _context;
        private readonly IGHNService _gHNService;
        private readonly IVNPayService _vNPayService;
        private readonly IStoredService _storedService;
        private readonly IStoreService _storeService;

        public OrderService(HousewareContext context, IGHNService gHNService, IVNPayService vNPayService, IStoredService storedService, IStoreService storeService)
        {
            _context = context;
            _gHNService = gHNService;
            _vNPayService = vNPayService;
            _storedService = storedService;
            _storeService = storeService;
        }

        public Response CreateOrder(CreateOrderRequest model)
        {
            Response response = new();
            try
            {
                var customer = _context.Customers.Where(c => c.CustomerId == model.CustomerId).FirstOrDefault();
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Customer does not exist!");
                    return response;
                }

                var address = _context.Addresses.Where(a => a.AddressId == model.AddressId && a.CustomerId == model.CustomerId).FirstOrDefault();
                if (address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This address could not be found in the customer's address book");
                    return response;
                }

                // Best story
                var getFeeResponse = _storeService.GetFeeStore(new GetFeeRequest()
                {
                    CustomerId = customer.CustomerId,
                    AddressId = address.AddressId
                });
                if (getFeeResponse.ResultCode != 0)
                {
                    return getFeeResponse;
                }
                GetCalculateFee calculateFee = (GetCalculateFee)getFeeResponse.Result;

                // Calculate real fee
                GHNCreateOrderRequest createOrderRequest = new();
                _context.Entry(customer).Collection(c => c.Carts).Query().Include(ca => ca.Product).Load();
                if (customer.Carts == null || customer.Carts.Count == 0)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This cart is empty!");
                    return response;
                }
                var store = _context.Stores.Where(s => s.StoreId == calculateFee.Store.StoreId).FirstOrDefault();
                if (store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Store does not exist!");
                    return response;
                }

                createOrderRequest.SetValueProduct(customer.Carts.ToList());
                createOrderRequest.SetValueAddress(address);
                createOrderRequest.SetValueStore(store);
                createOrderRequest.To_name = customer.FullName ?? GlobalVariable.GHNToNameDefault;
                createOrderRequest.Payment_type_id = 2;
                GHNResponse previewOrderResponse = new();
                previewOrderResponse = _gHNService.PreviewOrder(createOrderRequest, store.ShopId);
                if (previewOrderResponse.Code != 200)
                {
                    response.SetCode(CodeTypes.Err_Exception);
                    response.SetResult("Can't get preview order! " + previewOrderResponse.Code_message);
                    return response;
                }

                // Create order
                Order order = new()
                {
                    CustomerId = customer.CustomerId,
                    AddressId = address.AddressId,
                    StoreId = store.StoreId,
                    OrderStatus = GlobalVariable.OrderOrdered,
                    Fee = previewOrderResponse.Data.Value<int>("total_fee"),
                    Note = model.Note
                };

                // Move list product from cart into order detail
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
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
                        _context.Carts.RemoveRange(customer.Carts);
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        response.SetCode(CodeTypes.Err_AccFail);
                        response.SetResult("Create order failed! " + e.Message);
                        return response;
                    }
                }

                response.SetCode(CodeTypes.Success);
                response.SetResult(order);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response GetResutlOrder(OrderIdRequest model)
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

                _context.Entry(order).Reference(o => o.Address).Load();
                if (order.Address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Address does not exist!");
                    return response;
                }

                _context.Entry(order).Reference(o => o.Store).Load();
                if (order.Store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Store does not exist!");
                    return response;
                }

                GetOrderResponse orderResponse = new(order)
                {
                    Store = new StoreResponse(order.Store),
                    Address = new AddressResponse(order.Address)
                };
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

        public Response CreateOrderOffline(CreateOrderRequest model)
        {
            Response response = CreateOrder(model);
            if (response.ResultCode == 0)
            {
                Order order = (Order)response.Result;
                order.PaymentType = GlobalVariable.PayCod;
                order.OrderStatus = GlobalVariable.OrderProcessing;
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();
                response = GetResutlOrder(new OrderIdRequest(order.OrderId));
            }
            return response;
        }

        public Response CreateOrderOnline(CreateOrderOnlineRequest model)
        {
            Response response = CreateOrder(model);
            if (response.ResultCode == 0)
            {
                Order order = (Order)response.Result;
                order.PaymentType = GlobalVariable.PayOnline;
                order.OrderStatus = GlobalVariable.OrderPaymenting;
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();
                _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();

                try
                {
                    var url = _vNPayService.GeneratePaymentURL(model.ReturnUrl, order.OrderId, order.OrderDetails.Select(od => od.Product.Price * od.Quantity).Sum() + order.Fee);
                    response.SetResult(url);
                }
                catch (Exception e)
                {
                    response.SetCode(CodeTypes.Err_Exception);
                    response.SetResult("Create order failed! " + e.Message);
                }
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
                _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();
                if (order != null)
                {
                    if (order.OrderDetails.Select(od => od.Product.Price * od.Quantity).Sum() + order.Fee == model.Vnp_Amount/100)
                    {
                        if (model.Vnp_ResponseCode == "00" && model.Vnp_TransactionStatus == "00")
                        {
                            order.OrderStatus = GlobalVariable.OrderProcessing;
                            order.TransactionNo = model.Vnp_TransactionNo;
                            _context.Entry(order).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                        response.RspCode = "00";
                        response.Message = "Confirm Success";
                    }
                    else
                    {
                        response.RspCode = "04";
                        response.Message = "Invalid amount";
                    }
                }
                else
                {
                    response.RspCode = "01";
                    response.Message = "Order not found";
                }
            }
            else
            {
                response.RspCode = "97";
                response.Message = "Invalid signature";
            }
            return response;
        }

        public Response GetOrders(GetOrdersRequest model)
        {
            Response response = new();
            try
            {
                var customer = _context.Customers.Where(o => o.CustomerId == model.CustomerId).FirstOrDefault();
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Customer does not exist!");
                    return response;
                }

                List<Order> orders = new();
                switch (model.Status != null ? model.Status.ToUpper() : model.Status)
                {
                    case GlobalVariable.OrderPaymenting:
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId && o.OrderStatus == GlobalVariable.OrderPaymenting).ToList();
                        break;
                    case GlobalVariable.OrderProcessing:
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId && o.OrderStatus == GlobalVariable.OrderProcessing).ToList();
                        break;
                    case GlobalVariable.OrderDoing:
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId && o.OrderStatus == GlobalVariable.OrderDoing).ToList();
                        break;
                    case GlobalVariable.OrderDone:
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId && o.OrderStatus == GlobalVariable.OrderDone).ToList();
                        break;
                    case GlobalVariable.OrderCancel:
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId && o.OrderStatus == GlobalVariable.OrderCancel).ToList();
                        break;
                    case null:
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId).ToList();
                        break;
                }

                GetOrderPagingResponse orderPagingResponse = new()
                {
                    Page = model.Page,
                    Size = model.Size,
                    TotalPage = (int)Math.Ceiling((decimal)orders.Count / model.Size)
                };

                orders = orders.OrderBy(o => o.OrderDate)
                    .Skip(model.Size * model.Page)
                    .Take(model.Size)
                    .ToList();

                if (orders != null && orders.Count > 0)
                {
                    foreach (var order in orders)
                    {
                        _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();

                        if (order.OrderStatus == GlobalVariable.OrderDoing)
                        {
                            var orderInfo = _gHNService.OrderInfo(new GHNOrderInfoRequest(order.OrderCode));
                            if (orderInfo.Code == 200)
                            {
                                JObject jData = JObject.FromObject(orderInfo.Data);
                                var status = jData.Value<string>("status");
                                if (status == "cancel")
                                {
                                    order.OrderStatus = GlobalVariable.OrderCancel;
                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                                else if (status == "delivered")
                                {
                                    order.OrderStatus = GlobalVariable.OrderDone;
                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        var orderRes = new GetOrderResponse(order);
                        orderPagingResponse.Orders.Add(orderRes);
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(orderPagingResponse);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response GetOrdersAdmin(GetOrdersAdminRequest model)
        {
            Response response = new();
            try
            {
                List<Order> orders = new();
                switch (model.Status != null ? model.Status.ToUpper() : model.Status)
                {
                    case GlobalVariable.OrderPaymenting:
                        orders = _context.Orders.Where(o => o.OrderStatus == GlobalVariable.OrderPaymenting).ToList();
                        break;
                    case GlobalVariable.OrderProcessing:
                        orders = _context.Orders.Where(o => o.OrderStatus == GlobalVariable.OrderProcessing).ToList();
                        break;
                    case GlobalVariable.OrderDoing:
                        orders = _context.Orders.Where(o => o.OrderStatus == GlobalVariable.OrderDoing).ToList();
                        break;
                    case GlobalVariable.OrderDone:
                        orders = _context.Orders.Where(o => o.OrderStatus == GlobalVariable.OrderDone).ToList();
                        break;
                    case GlobalVariable.OrderCancel:
                        orders = _context.Orders.Where(o => o.OrderStatus == GlobalVariable.OrderCancel).ToList();
                        break;
                    case null:
                        orders = _context.Orders.ToList();
                        break;
                }

                GetOrderPagingResponse orderPagingResponse = new()
                {
                    Page = model.Page,
                    Size = model.Size,
                    TotalPage = (int)Math.Ceiling((decimal)orders.Count / model.Size)
                };

                orders = orders.OrderBy(o => o.OrderDate)
                    .Skip(model.Size * model.Page)
                    .Take(model.Size)
                    .ToList();

                if (orders != null && orders.Count > 0)
                {
                    foreach (var order in orders)
                    {
                        _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();

                        if (order.OrderStatus == GlobalVariable.OrderDoing)
                        {
                            var orderInfo = _gHNService.OrderInfo(new GHNOrderInfoRequest(order.OrderCode));
                            if (orderInfo.Code == 200)
                            {
                                JObject jData = JObject.FromObject(orderInfo.Data);
                                var status = jData.Value<string>("status");
                                if (status == "cancel")
                                {
                                    order.OrderStatus = GlobalVariable.OrderCancel;
                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                                else if (status == "delivered")
                                {
                                    order.OrderStatus = GlobalVariable.OrderDone;
                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                        }
                        var orderRes = new GetOrderResponse(order);
                        orderPagingResponse.Orders.Add(orderRes);
                    }
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(orderPagingResponse);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
            }
            return response;
        }

        public Response CreateOrderGHN(OrderIdRequest model)
        {
            Response response = new();
            try
            {
                GHNCreateOrderRequest createOrderRequest = new();
                var order = _context.Orders.Where(o => o.OrderId == model.OrderId).FirstOrDefault();
                if (order == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Order does not exist!");
                    return response;
                }
                createOrderRequest.Client_order_code = order.OrderId.ToString();

                var customer = _context.Customers.Where(c => c.CustomerId == order.CustomerId).FirstOrDefault();
                if (customer == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Customer does not exist!");
                    return response;
                }
                createOrderRequest.To_name = customer.FullName ?? GlobalVariable.GHNToNameDefault;

                _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(od => od.Product).Load();
                if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This order is empty!");
                    return response;
                }
                createOrderRequest.SetValueProduct(order.OrderDetails.ToList());

                _context.Entry(order).Reference(o => o.Address).Load();
                if (order.Address == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Address does not exist!");
                    return response;
                }
                createOrderRequest.SetValueAddress(order.Address);

                _context.Entry(order).Reference(o => o.Store).Load();
                if (order.Store == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("Store does not exist!");
                    return response;
                }
                createOrderRequest.SetValueStore(order.Store);

                if (!_storedService.DecreaseStored(order.Store, order.OrderDetails.ToList()))
                {
                    throw new Exception("Can't decrease stored");
                }

                // Create order GHN
                createOrderRequest.Cod_amount = createOrderRequest.Insurance_value;
                createOrderRequest.Content = GlobalVariable.GHNContent(createOrderRequest.Client_order_code);
                createOrderRequest.Payment_type_id = 2;
                createOrderRequest.Note = order.Note;

                var createOrderResponse = _gHNService.CreateOrder(createOrderRequest, order.Store.ShopId);

                order.OrderCode = createOrderResponse.Data.Value<string>("order_code");
                order.OrderStatus = GlobalVariable.OrderDoing;
                order.Fee = createOrderResponse.Data.Value<int>("total_fee");
                _context.Entry(order).State = EntityState.Modified;
                _context.SaveChanges();

                // Create response
                response = GetResutlOrder(new OrderIdRequest(order.OrderId));
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
