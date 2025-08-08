namespace QuanLyGameConsole.Models.ViewModels
{
    public class CartRequest
    {
        public int ProductId { get; set; }

        public string Slug { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Quantity { get; set; }

        public double Total => Quantity * Price;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
