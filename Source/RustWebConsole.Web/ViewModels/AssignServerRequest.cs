using System.ComponentModel.DataAnnotations;

namespace RustWebConsole.Web.ViewModels
{
    public class AssignServerRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int ServerId { get; set; }
    }
}