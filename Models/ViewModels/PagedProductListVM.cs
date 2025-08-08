namespace QuanLyGameConsole.Models.ViewModels
{
    public class PagedProductListVM
    {
        public IEnumerable<ProductVM> Products { get; set; } = null!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
