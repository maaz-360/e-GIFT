using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eGift.Admin.CustomFilter;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(
        ActionExecutingContext context)
    {
        // Allow actions marked with [AllowAnonymous]
        var endpoint = context.HttpContext.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            return;
        }
        
        var userId = context.HttpContext.Session.GetInt32("UserId");

        if (!userId.HasValue)
        {
            context.Result = new RedirectToActionResult(
                "Index",
                "Account",
                null);
        }

        base.OnActionExecuting(context);
    }
}