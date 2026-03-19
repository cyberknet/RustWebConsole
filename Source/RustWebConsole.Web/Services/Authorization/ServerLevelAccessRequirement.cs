using Microsoft.AspNetCore.Authorization;

namespace RustWebConsole.Web.Services.Authorization
{
    public class ServerLevelAccessRequirement : IAuthorizationRequirement
    {
        public ServerLevelAccessRequirement() { }
    }
}