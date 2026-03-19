using Microsoft.AspNetCore.Authorization;

namespace RustWebConsole.Web.Services.Authorization
{
    public class ServerAccessRequirement : IAuthorizationRequirement
    {
        public ServerAccessRequirement() { }
    }
}