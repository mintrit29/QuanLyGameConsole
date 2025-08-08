using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Components
{
    public class FeaturedProductVIewComponent : ViewComponent
    {
        private readonly GameConsoleContext _context;

        public FeaturedProductVIewComponent(GameConsoleContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            DateTime cutoffDate = DateTime.Now.AddDays(-30);
            var featureProduct = await _context.Products
                .Where(p => p.CreatedAt >= cutoffDate)
                .Select(p => new ProductVM()
                {
                    Slug = p.Slug,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    ProductRating = p.ProductRatings.Any()
                        ? p.ProductRatings.Average(r => (double)r.Rating!)
                        : 0,
                }).ToListAsync();
            ViewBag.FeaturedProduct = featureProduct;
            return View();
        }
    }
}
