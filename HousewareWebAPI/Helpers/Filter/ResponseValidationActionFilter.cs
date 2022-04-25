using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace HousewareWebAPI.Helpers.Filter
{
    public class ResponseValidationActionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = 0;
        public void OnActionExecuting(ActionExecutingContext context) {
            if (!context.ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                var reponse = new Response(CodeTypes.Err_CustomValidation, errors);
                context.Result = new BadRequestObjectResult(reponse)
                {
                    StatusCode = 400
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
