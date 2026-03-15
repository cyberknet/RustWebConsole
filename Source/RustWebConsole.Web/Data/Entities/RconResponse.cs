namespace RustWebConsole.Web.Data.Entities
{
    public class RconResponse
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public RconRequest Request { get; set; } = null!;
        public string Response { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}