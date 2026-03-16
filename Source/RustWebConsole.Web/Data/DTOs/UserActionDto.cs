namespace RustWebConsole.Web.Data.DTOs
{
    public class UserActionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Target { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; }
        public string ActionType { get; set; } = string.Empty;
    }
}