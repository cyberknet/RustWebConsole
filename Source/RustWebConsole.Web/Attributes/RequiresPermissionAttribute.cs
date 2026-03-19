using RustWebConsole.Web.Data.Enums;
using System;

namespace RustWebConsole.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresPermissionAttribute : Attribute
    {
        public PermissionLevel Permission { get; }

        public RequiresPermissionAttribute(PermissionLevel permission)
        {
            Permission = permission;
        }
    }
}