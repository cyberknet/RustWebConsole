using System.ComponentModel.DataAnnotations;

namespace RustWebConsole.Web.Data.Entities
{
    public class PlayerInventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }

        [Required]
        public Player Player { get; set; } = null!;

        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        public string? Metadata { get; set; } // Optional field for additional item details
    }
}