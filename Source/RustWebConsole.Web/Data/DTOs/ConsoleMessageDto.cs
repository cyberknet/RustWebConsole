namespace RustWebConsole.Web.Data.DTOs
{
    public class ConsoleMessageDto
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}