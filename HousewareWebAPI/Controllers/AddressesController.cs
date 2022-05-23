using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("getaddress")]
        public IActionResult GetAddess([FromBody] GetAddressRequest model)
        {
            var response = _addressService.GetAddress(model.AddressId);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("getaddresses")]
        public IActionResult GetAddesses([FromBody] GetAddressesRequest model)
        {
            var response = _addressService.GetAddresses(model.CustomerId);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("addaddress")]
        public IActionResult AddAddress(AddAddressRequest model)
        {
            var response = _addressService.AddAddress(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("updateaddress")]
        public IActionResult UpdateAddress(UpdateAddressRequest model)
        {
            var response = _addressService.UpdateAddress(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("deleteaddress")]
        public IActionResult DeleteAddress(GetAddressRequest model)
        {
            var response = _addressService.DeleteAddress(model.AddressId);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
