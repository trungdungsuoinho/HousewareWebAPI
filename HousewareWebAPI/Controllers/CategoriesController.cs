using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HousewareWebAPI.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get a Category. API for client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            var response = _categoryService.GetCategory(id);
            if (response == null) return BadRequest(CodeTypes.Err_Unknown);
            if (response.ResultCode != CodeTypes.Success.ResultCode) return BadRequest(response);
            return Ok(response);
        }
    }
}
