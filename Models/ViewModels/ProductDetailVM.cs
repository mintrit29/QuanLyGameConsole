namespace QuanLyGameConsole.Models.ViewModels
{
    public class ProductDetailVM
    {
        public Product Product { get; set; } = null!;

        public List<Product> RelatedProducts { get; set; } = new List<Product>();
        public double ProductRating { get; set; }
        public int TotalRating { get; set; }
        public List<ProductCommentVM> Comments { get; set; } = new List<ProductCommentVM>();

    }
}
