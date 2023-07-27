using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Extensions;

public static class ExceptionContextExtensions
{
    public static ExceptionContext SetStatusCode(this ExceptionContext context, int code)
    {
        context.HttpContext.Response.StatusCode = code;

        return context;
    }

    public static ExceptionContext SetResult(this ExceptionContext context, object obj)
    {
        context.Result = new JsonResult(obj);

        return context;
    }
}
