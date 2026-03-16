namespace RustWebConsole.Web.Data.DTOs
{
    public class PlayerInventoryDto
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? Metadata { get; set; }
    }
}