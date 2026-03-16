namespace RustWebConsole.Web.Data.Entities
{
    public class PlayerStatistics
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public TimeSpan PlayTime { get; set; }

        // No additional relationships needed
    }
}