using Houseware.WebAPI.Data;
using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HousewareWebAPI.Services
{
    public interface ICartService
    {
        public Response GetCart(Guid customerId);
        public Response AddProIntoCart(AddCartRequest model);
        public Response UpdateProInCart(AddCartRequest model);
        public Response DeleteProInCart(DeleteCartRequest model);
    }

    public class CartService : ICartService
    {
        private readonly HousewareContext _context;
        private readonly IProductService _productService;

        public CartService(HousewareContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        private Cart GetCart(Guid customerId, string productId)
        {
            return _context.Carts.Where(c => c.CustomerId == customerId && c.ProductId == productId).FirstOrDefault();
        }

        private List<Cart> GetCartsByCusId(Guid id)
        {
            var carts = _context.Carts
                .Where(c => c.CustomerId == id)
                .Include(c => c.Product)
                .ToList();
            return carts;
        }

        public Response GetCart(Guid customerId)
        {
            Response response = new();
            try
            {
                CartResponse cartResponse = new()
                {
                    CustomerId = customerId,
                    Product = new()
                };
                var carts = GetCartsByCusId(customerId);
                if (carts != null || carts.Count > 0)
                {
                    foreach (var cart in carts)
                    {
                        cartResponse.Product.Add(new ProInCartResponse
                        {
                            ProductId = cart.ProductId,
                            Name = cart.Product.Name,
                            Avatar = cart.Product.Avatar,
                            Price = cart.Product.Price,
                            Quantity = cart.Quantity,
                            ItemPrice = cart.Product.Price * cart.Quantity
                        });
                    }
                    cartResponse.TotalPrice = (uint)cartResponse.Product.Sum(p => p.ItemPrice);
                }
                response.SetCode(CodeTypes.Success);
                response.SetResult(cartResponse);
                return response;
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response AddProIntoCart(AddCartRequest model)
        {
            Response response = new();
            try
            {
                var cart = GetCart(model.CustomerId, model.ProductId);
                if (cart != null)
                {
                    cart.Quantity += model.Quantity;
                    _context.Entry(cart).State = EntityState.Modified;
                }
                else
                {
                    _context.Carts.Add(new Cart
                    {
                        CustomerId = model.CustomerId,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity
                    });                    
                }
                _context.SaveChanges();

                return GetCart(model.CustomerId);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response UpdateProInCart(AddCartRequest model)
        {
            Response response = new();
            try
            {
                if (model.Quantity == 0)
                {
                    return DeleteProInCart(model);
                }

                Cart cart = new();
                cart = GetCart(model.CustomerId, model.ProductId);
                if (cart == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This product does not exist in the cart");
                    return response;
                }
                if (cart.Quantity != model.Quantity)
                {
                    cart.Quantity = model.Quantity;
                    _context.Entry(cart).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return GetCart(model.CustomerId);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }

        public Response DeleteProInCart(DeleteCartRequest model)
        {
            Response response = new();
            try
            {
                var cart = GetCart(model.CustomerId, model.ProductId);
                if (cart == null)
                {
                    response.SetCode(CodeTypes.Err_NotExist);
                    response.SetResult("This product does not exist in the cart");
                    return response;
                }
                _context.Carts.Remove(cart);
                _context.SaveChanges();
                return GetCart(model.CustomerId);
            }
            catch (Exception e)
            {
                response.SetCode(CodeTypes.Err_Exception);
                response.SetResult(e.Message);
                return response;
            }
        }
    }
}
