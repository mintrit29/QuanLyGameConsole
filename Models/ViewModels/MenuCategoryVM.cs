namespace QuanLyGameConsole.Models.ViewModels
{
    public class MenuCategoryVM
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? ParentId { get; set; }
        public string? Slug { get; set; }
    }
}
