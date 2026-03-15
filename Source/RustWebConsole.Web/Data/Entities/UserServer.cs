namespace RustWebConsole.Web.Data.Entities
{
    public class UserServer
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public int ServerId { get; set; }
        public Server Server { get; set; } = null!;
    }
}