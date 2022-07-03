using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Models;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create/cod")]
        public IActionResult CreateOrderShipCod([FromBody] CreateOrderRequest model)
        {
            var response = _orderService.CreateOrderOffline(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("create/onl")]
        public IActionResult CreateOrderPayment([FromBody] CreateOrderOnlineRequest model)
        {
            var response = _orderService.CreateOrderOnline(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("callback/ipn")]
        public IActionResult CallbackVNPayPayment([FromQuery] CodeIPNURLRequest model)
        {
            var response = _orderService.IPNVNPay(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            return Ok(response);
        }

        [HttpPost("preview")]
        public IActionResult GetOrderPayment([FromBody] GetPreviewOrderRequest model)
        {
            var response = _orderService.GetResutlOrderOnline(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("get")]
        public IActionResult GetListOrder([FromBody] GetOrdersRequest model)
        {
            var response = _orderService.GetOrders(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
