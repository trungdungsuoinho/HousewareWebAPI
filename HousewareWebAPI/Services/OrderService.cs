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
        public Response CreateOrderOffline(CreateOrderRequest model);
        public Response CreateOrderOnline(CreateOrderOnlineRequest model);
        public IPNVNPayResponse IPNVNPay(CodeIPNURLRequest model);
        public Response GetResutlOrderOnline(OrderIdRequest model);
        public Response GetOrders(GetOrdersRequest model);
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

        private List<OrderDetail> GetOrderDetails(Guid orderId)
        {
            return _context.OrderDetails.Where(o => o.OrderId == orderId).Include(o => o.Product).ToList();
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

                var store = _storeService.GetFeeStore(new GetFeeRequest()
                {
                    CustomerId = customer.CustomerId,
                    AddressId = address.AddressId
                });

                if (store.ResultCode != 0)
                {
                    return store;
                }

                // Create order and move list product from cart into order detail                
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    GetCalculateFee calculateFee = (GetCalculateFee)store.Result;
                    Order order = new()
                    {
                        CustomerId = model.CustomerId,
                        AddressId = model.AddressId,
                        StoreId = calculateFee.Store.StoreId,
                        OrderStatus = GlobalVariable.OrderOrdered,
                        Note = model.Note
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    _context.Entry(customer).Collection(c => c.Carts).Load();
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

                    // Create response
                    _context.Entry(order).Collection(o => o.OrderDetails).Query().Include(o => o.Product).Load();
                    CreateOrderResponse orderResponse = new(order)
                    {
                        Store = calculateFee.Store,
                        Address = new AddressResponse(address),
                        TotalFee = calculateFee.Fee
                    };
                    orderResponse.Total = orderResponse.TotalPrice + orderResponse.TotalFee;

                    order.Amount = orderResponse.Total;
                    _context.Entry(order).State = EntityState.Modified;
                    _context.SaveChanges();
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

        public Response CreateOrderOffline(CreateOrderRequest model)
        {
            Response response = CreateOrder(model);
            if (response.ResultCode == 0)
            {
                CreateOrderResponse createOrder = (CreateOrderResponse)response.Result;
                var order = _context.Orders.Where(o => o.OrderId == createOrder.OrderId).FirstOrDefault();
                if (order != null)
                {
                    order.PaymentType = GlobalVariable.PayCod;
                    order.OrderStatus = GlobalVariable.OrderProcessing;
                    _context.Entry(order).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            return response;
        }

        public Response CreateOrderOnline(CreateOrderOnlineRequest model)
        {
            Response response = CreateOrder(model);
            if (response.ResultCode == 0)
            {
                CreateOrderResponse createOrder = (CreateOrderResponse)response.Result;
                var order = _context.Orders.Where(o => o.OrderId == createOrder.OrderId).FirstOrDefault();
                if (order != null)
                {
                    order.PaymentType = GlobalVariable.PayOnline;
                    order.OrderStatus = GlobalVariable.OrderPaymenting;
                    _context.Entry(order).State = EntityState.Modified;
                    _context.SaveChanges();

                    try
                    {
                        var url = _vNPayService.GeneratePaymentURL(model.ReturnUrl, order.OrderId, order.Amount);
                        response.SetResult(url);
                    }
                    catch (Exception e)
                    {
                        response.SetCode(CodeTypes.Err_Exception);
                        response.SetResult("Create order failed! " + e.Message);
                    }
                }
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

                // Create order GHN
                createOrderRequest.Client_order_code = order.OrderId.ToString();
                createOrderRequest.Cod_amount = createOrderRequest.Insurance_value;
                createOrderRequest.Content = GlobalVariable.GHNContent(createOrderRequest.Client_order_code);
                createOrderRequest.Payment_type_id = 2;
                createOrderRequest.Note = order.Note;

                var jObjCreateOrder = _gHNService.CreateOrder(createOrderRequest, order.Store.ShopId);

                order.OrderCode = jObjCreateOrder.Value<string>("order_code");
                order.OrderStatus = GlobalVariable.OrderDoing;
                order.Amount = createOrderRequest.Insurance_value + jObjCreateOrder.Value<int>("total_fee");
                _context.Entry(order).State = EntityState.Modified;

                if (!_storedService.DecreaseStored(order.Store, order.OrderDetails.ToList()))
                {
                    throw new Exception("Can't decrease stored");
                }
                _context.SaveChanges();

                // Create response
                GetOrderResponse orderResponse = new()
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    Store = new StoreResponse(order.Store),
                    Address = new AddressResponse(order.Address),
                    TotalPrice = createOrderRequest.Insurance_value,
                    TotalFee = jObjCreateOrder.Value<int>("total_fee"),
                    Total = order.Amount,
                    ExpectedDeliveryTime = jObjCreateOrder.Value<DateTime>("expected_delivery_time")
                };
                var orderDetails = _context.OrderDetails.Where(o => o.OrderId == order.OrderId).Include(o => o.Product).ToList();
                foreach (var orderDetail in orderDetails)
                {
                    orderResponse.Products.Add(new ProductInCartResponse
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
                            _context.Entry(order).Collection(o => o.OrderDetails).Load();
                            _storedService.DecreaseStored(store, order.OrderDetails.ToList());
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

        public Response GetResutlOrderOnline(OrderIdRequest model)
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

                GetOrderResponse orderResponse = new()
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    Store = new StoreResponse(store),
                    Address = new AddressResponse(address),
                    TotalPrice = jObjGetOrder.Value<int>("insurance_value"),
                    TotalFee = order.Amount - jObjGetOrder.Value<int>("insurance_value"),
                    Total = order.Amount,
                    ExpectedDeliveryTime = jObjGetOrder.Value<DateTime>("leadtime")
                };
                foreach (var orderDetail in order.OrderDetails)
                {
                    orderResponse.Products.Add(new ProductInCartResponse
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
                        orders = _context.Orders.Where(o => o.CustomerId == model.CustomerId && (o.OrderStatus == GlobalVariable.OrderOrdered || o.OrderStatus == GlobalVariable.OrderPaymented)).ToList();
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
                        if (orderRes.Status == GlobalVariable.OrderOrdered || orderRes.Status == GlobalVariable.OrderPaymented)
                        {
                            orderRes.Status = GlobalVariable.OrderProcessing;
                        }
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
    }
}
