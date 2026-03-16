using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RustWebConsole.Web.Data.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string SteamId { get; set; } = string.Empty;

        [Required]
        public int ServerId { get; set; }

        [ForeignKey(nameof(ServerId))]
        public Server Server { get; set; } = null!;

        // Relationships
        public ICollection<PlayerStatistics> PlayerStatistics { get; set; } = new List<PlayerStatistics>();
    }
}