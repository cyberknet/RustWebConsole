namespace RustWebConsole.Web.Data.DTOs
{
    public class RconResponseDto
    {
        public int Id { get; set; }
        public int RconRequestId { get; set; }
        public string ResponseData { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}