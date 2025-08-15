namespace QuanLyGameConsole.Models.ViewModels
{
    public class ProductCommentVM
    {
        public string? CustomerName { get; set; }
        public string? Content { get; set; }
        public string? Reply { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Rating { get; set; }
    }
}
