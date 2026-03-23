using RustWebConsole.Web.Data.Enums;

namespace RustWebConsole.Web.Data.Enums
{
    public enum ActionType
    {
        Register,
        Login,
        Logout, // Added Logout action type
        RefreshToken,
        ExternalLoginAttempt, // Added ExternalLoginAttempt action type
        ExternalLoginFailure // Added ExternalLoginFailure action type
    }
}