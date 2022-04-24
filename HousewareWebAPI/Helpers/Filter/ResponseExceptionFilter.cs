using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HousewareWebAPI.Helpers.Filter
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var reponse = new Reponse(CodeTypes.Err_Exception, context.Exception.Message);
                context.Result = new ObjectResult(reponse)
                {
                    StatusCode = 400
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
