using Identity.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.Shared.Models;

namespace Identity.Filters;

public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    public const string DefaultMessage = "An error occurred while processing the request.";

    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidCredentialsException)
        {
            context
                .SetStatusCode(StatusCodes.Status401Unauthorized)
                .SetResult(AuthenticationResult.InvalidCredentials(context.Exception.Message));
        }
        else
        {
            context
                .SetStatusCode(StatusCodes.Status500InternalServerError)
                .SetResult(AuthenticationResult.Error(DefaultMessage));
        }

        base.OnException(context);
    }
}
