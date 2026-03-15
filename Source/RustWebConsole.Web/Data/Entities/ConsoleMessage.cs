namespace RustWebConsole.Web.Data.Entities
{
    public class ConsoleMessage
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public Server Server { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}