using Microsoft.AspNetCore.Authorization;
using RustWebConsole.Web.Data;
using RustWebConsole.Web.Data.Entities;
using System.Security.Claims;

namespace RustWebConsole.Web.Services.Authorization
{
    public class ServerAccessHandler : AuthorizationHandler<ServerAccessRequirement>
    {
        private readonly ApplicationDbContext _dbContext;

        public ServerAccessHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ServerAccessRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Task.CompletedTask;
            }

            var serverId = context.Resource as int?;
            if (serverId == null)
            {
                return Task.CompletedTask;
            }

            var hasAccess = _dbContext.UserServers.Any(us => us.UserId == userId && us.ServerId == serverId);
            if (hasAccess)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}