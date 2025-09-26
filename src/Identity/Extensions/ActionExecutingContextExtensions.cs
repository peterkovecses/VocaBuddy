namespace Identity.Extensions;

public static class ActionExecutingContextExtensions
{
    public static KeyValuePair<string, object>[] GetModelStateErrors(this ActionExecutingContext context) =>
        context.ModelState.Where(x => x.Value is
            {
                Errors.Count: > 0
            })
            .Select(x => new KeyValuePair<string, object>(x.Key,
                x.Value!.Errors.Select(e => e.ErrorMessage)))
            .ToArray();
}