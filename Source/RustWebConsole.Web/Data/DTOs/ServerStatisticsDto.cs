namespace RustWebConsole.Web.Data.DTOs
{
    public class ServerStatisticsDto
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public DateTime Timestamp { get; set; }
        public int PlayerCount { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
    }
}