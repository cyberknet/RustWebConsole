using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RustWebConsole.Web.Data.Entities
{
    public class ServerTag
    {
        [Key, Column(Order = 0)]
        public int ServerId { get; set; }

        [ForeignKey("ServerId")]
        public Server Server { get; set; } = null!;

        [Key, Column(Order = 1)]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; } = null!;
    }
}