using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Components
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly GameConsoleContext _context;
        public MenuCategoryViewComponent(GameConsoleContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _context.Categories.Select(c => new MenuCategoryVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ParentId = c.ParentId,
                Slug = c.Slug,
            }).ToListAsync();
            return View(category);
        }
    }
}
