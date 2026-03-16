namespace RustWebConsole.Web.Data.DTOs
{
    public class RconRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServerId { get; set; }
        public string Command { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}