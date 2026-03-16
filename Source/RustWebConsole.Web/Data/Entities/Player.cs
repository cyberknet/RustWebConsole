using System.Collections.Generic;

namespace RustWebConsole.Web.Data.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SteamId { get; set; } = string.Empty;

        // Relationships
        public ICollection<PlayerStatistics> PlayerStatistics { get; set; } = new List<PlayerStatistics>();
    }
}