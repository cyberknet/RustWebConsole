using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RustWebConsole.Web.Data.Entities
{
    public class RconResponse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RconRequestId { get; set; }

        [ForeignKey(nameof(RconRequestId))]
        public RconRequest RconRequest { get; set; } = null!;

        [Required]
        public string ResponseData { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}