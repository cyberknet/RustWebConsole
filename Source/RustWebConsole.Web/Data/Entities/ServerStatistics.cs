namespace RustWebConsole.Web.Data.Entities
{
    public class ServerStatistics
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public Server Server { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int PlayerCount { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
    }
}