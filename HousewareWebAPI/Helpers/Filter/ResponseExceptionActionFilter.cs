using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HousewareWebAPI.Helpers.Filter
{
    public class ResponseExceptionActionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = 1;
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var reponse = new Response(CodeTypes.Err_Exception, context.Exception.Message);
                context.Result = new BadRequestObjectResult(reponse)
                {
                    StatusCode = 400
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
