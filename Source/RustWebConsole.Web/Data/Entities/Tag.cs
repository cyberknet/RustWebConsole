using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RustWebConsole.Web.Data.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<ServerTag> ServerTags { get; set; } = new List<ServerTag>();
    }
}