using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get Cart by CustomerId. API for client
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost("getcart")]
        public IActionResult GetAllProductInCart([FromBody] GetCartRequest model)
        {
            var response = _cartService.GetCart(model.CustomerId);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Add Product into Cart. API for client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addcart")]
        public IActionResult AddProductIntoCart(AddProIntoCartRequest model)
        {
            var response = _cartService.AddProIntoCart(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Update Product in Cart. API for client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("updatecart")]
        public IActionResult UpdateProductInCart(AddProIntoCartRequest model)
        {
            var response = _cartService.UpdateProInCart(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete Product in Cart. API for client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("deletecart")]
        public IActionResult DeleteProductInCart(DeleteProInCartRequest model)
        {
            var response = _cartService.DeleteProInCart(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
