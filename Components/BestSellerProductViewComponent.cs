using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Components

{
    public class BestSellerProductViewComponent : ViewComponent
    {
        private readonly GameConsoleContext _context;

        public BestSellerProductViewComponent(GameConsoleContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bestSellerProduct = await _context.Products
                .Where(p => p.Views >= 1000)
                .Include(p => p.ProductRatings)
                .Select(p => new ProductVM()
                {
                    Slug = p.Slug,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Image = p.Image,
                    ProductRating = p.ProductRatings.Any()
                        ? p.ProductRatings.Average(r => (double)r.Rating!) : 0,
                }).ToListAsync();
            ViewBag.BestSellerProduct = bestSellerProduct;
            return View();
        }
    }
}
