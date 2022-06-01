using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet("{id}")]
        public IActionResult GetStore([FromRoute] int id)
        {
            var response = _storeService.GetStore(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetStores()
        {
            var response = _storeService.GetStores();
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddStore(AddStoreRequest model)
        {
            var response = _storeService.AddStore(model);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }

        //[HttpPut]
        //public IActionResult UpdateAddress(UpdateStoreRequest model)
        //{
        //    var response = _storeService.UpdateStore(model);
        //    if (response == null) return BadRequest(CodeTypes.Err_Unknown);
        //    if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
        //    return Ok(response);
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteAddress([FromRoute] int id)
        //{
        //    var response = _storeService.DeleteStore(id);
        //    if (response == null) return BadRequest(CodeTypes.Err_Unknown);
        //    if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
        //    return Ok(response);
        //}
    }
}
