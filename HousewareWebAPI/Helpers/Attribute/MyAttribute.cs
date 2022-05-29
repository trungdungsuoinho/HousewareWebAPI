using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using HousewareWebAPI.Helpers.Common;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HousewareWebAPI.Models;

namespace HousewareWebAPI.Helpers.Attribute
{
    public class MyRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("Field [{0}] is required", name);
        }
    }

    public class MyMaxLengthAttribute : MaxLengthAttribute
    {
        public MyMaxLengthAttribute(int length) : base(length)
        {
            base.ErrorMessageResourceName = "MaximumLength";
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Maximum length of field [{0}] is {1}", name, Length);
        }
    }

    public class MyMinLengthAttribute : MinLengthAttribute
    {
        public MyMinLengthAttribute(int length) : base(length)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Minimum length of field [{0}] is {1}", name, Length);
        }
    }

    public class MyRegularExpression : RegularExpressionAttribute
    {
        public MyRegularExpression(string pattern) : base(pattern)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The format of the [{0}] field is incorrect", name);
        }
    }

    public class MyRangeAttribute : RangeAttribute
    {
        public MyRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The value of field [{0}] must be between {1} and {2}", name, Minimum, Maximum);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorizeAttribute : System.Attribute, IAuthorizationFilter
    {
        //public AuthorizeAttribute(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //if (_appSettings.UsingJWT)
            {
                var role = context.HttpContext.Items["Role"];
                var id = context.HttpContext.Items["Id"];
                var issuedAt = context.HttpContext.Items["IssuedAt"];
                if (role == null || id == null || issuedAt == null)
                {
                    Response response = new();
                    response.SetCode(CodeTypes.Err_Unauthorized);
                    response.SetResult("Invalid token!");
                    context.Result = new JsonResult(response) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                //else if (DateTime.Now > DateTime.Parse(issuedAt.ToString()))
                //{
                //    context.Result = new JsonResult(CodeTypes.Err_Unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
                //}

                //if (role.ToString() == "Customer")
                //{
                //    var customer = (SUsrac)context.HttpContext.Items["User"];
                //    if (user == null)
                //    {
                //        context.Result = new JsonResult(CodeTypes.Err_Unauthorized) { StatusCode = StatusCodes.Status401Unauthorized };
                //    }
                //    else
                //    {
                //        if (user.expiretime != null)
                //        {
                //            DateTime expiryDate = (DateTime)user.expiretime;
                //            if (expiryDate < DateTime.Now)
                //            {
                //                context.Result = new JsonResult(Codetypes.UMG_INVALID_EXPDT) { StatusCode = StatusCodes.Status401Unauthorized };
                //            }
                //        }
                //    }
                //}
            }
        }
    }
}
