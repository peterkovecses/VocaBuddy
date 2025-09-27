namespace Identity.Web.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.GetModelStateErrors();
            var result = Result.ValidationError(errors);
            context.Result = new BadRequestObjectResult(result);
            
            return;
        }

        await next();
    }
}