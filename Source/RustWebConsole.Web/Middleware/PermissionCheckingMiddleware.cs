using Microsoft.AspNetCore.Http;
using RustWebConsole.Web.Attributes;
using RustWebConsole.Web.Data.Enums;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RustWebConsole.Web.Middleware
{
    public class PermissionCheckingMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionCheckingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var requiredPermissions = endpoint.Metadata.GetOrderedMetadata<RequiresPermissionAttribute>();
                if (requiredPermissions != null)
                {
                    var user = context.User;
                    if (user.Identity?.IsAuthenticated == true)
                    {
                        foreach (var permission in requiredPermissions)
                        {
                            var userPermission = Enum.TryParse<PermissionLevel>(
                                user.FindFirstValue("PermissionLevel"), out var parsedPermission)
                                ? parsedPermission
                                : PermissionLevel.Viewer;

                            if (userPermission < permission.Permission)
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                await context.Response.WriteAsync("Forbidden: You do not have the required permissions.");
                                return;
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}