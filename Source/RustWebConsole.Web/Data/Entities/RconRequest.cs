namespace RustWebConsole.Web.Data.Entities
{
    public class RconRequest
    {
        public int Id { get; set; }
        public string Command { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }
}