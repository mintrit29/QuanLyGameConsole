using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Components
{
    public class MenuBrandViewComponent : ViewComponent
    {
        private readonly GameConsoleContext _context;
        public MenuBrandViewComponent(GameConsoleContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuBrand = await _context.Brands.Select(b => new MenuBrandVM
            {
                BrandId = b.BrandId,
                Name = b.Name,
                Slug = b.Slug
            }).ToListAsync();
            return View(menuBrand);
        }
    }
}
