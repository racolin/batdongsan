using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using WebUI.Controllers.Apis;
using Application.Common.Responses;
using WebUI.Areas.Admin.Controllers.Apis;

namespace WebUI.Filters;
public class ApiAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {

        // Return status code for APIs response
        if (policy.Requirements.Any(x => x.GetType() == typeof(DenyAnonymousAuthorizationRequirement))
            && context.User.Identity?.IsAuthenticated == false)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(DataResponse<string>.Error("Hãy đăng nhập để có thể thực hiện thao tác!"));

            return;
        }

        // If the authorization was forbidden and the resource had a specific requirement,
        // provide a custom 404 response.
        // If the authorization was forbidden and the resource had a specific requirement,
        // provide a custom 404 response.
        if (authorizeResult.Forbidden)
        {
            var controller = context.GetRouteData().Values["controller"]?.ToString();
            if (string.IsNullOrWhiteSpace(controller))
            {
                // Return a 404 to make it appear as if the resource doesn't exist.
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(DataResponse<string>.Error("Không thể tìm thấy dữ liệu!!"));

                context.Response.Redirect(new PathString("/PageNotFound"));
                return;
            }

            var controllerType = Type.GetType($"{typeof(ApiControllerBase).Namespace}.{controller}Controller");
            if (controllerType != null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                await context.Response.WriteAsJsonAsync(DataResponse<string>.Error("Bạn không được phép thực hiện thao tác này!"));

                return;
            }

            var controllerAdminType = Type.GetType($"{typeof(ApiAdminControllerBase).Namespace}.{controller}Controller");
            if (controllerAdminType != null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                await context.Response.WriteAsJsonAsync(DataResponse<string>.Error("Bạn không được phép thực hiện thao tác này!"));

                return;
            }
        }

        // Fall back to the default implementation.
        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}

