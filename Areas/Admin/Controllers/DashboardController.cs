using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class DashboardController : Controller
    {
        private readonly GameConsoleContext _context;
        private readonly IWebHostEnvironment _webhostEnvironment;
        public DashboardController(GameConsoleContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webhostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var footerVM = new FooterVM
            {
                Footer = await _context.Footers.FirstOrDefaultAsync(),
            };
            ViewBag.customerCount = await _context.Customers.CountAsync();
            ViewBag.productCount = await _context.Products.Where(p => p.Status == 1).CountAsync();
            ViewBag.orderCount = await _context.Bills.Where(b => b.Status == 2).CountAsync();
            return View(footerVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(FooterVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "File không hợp lệ. Chỉ cho phép ảnh có đuôi là jpg, png, jpeg, gif và bmp";
                return RedirectToAction("Index");
            }

            try
            {
                if (model.LogoFile != null)
                {


                    string uploadsFolder = Path.Combine(_webhostEnvironment.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Đảm bảo thư mục tồn tại
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.LogoFile.CopyTo(fileStream);
                    }
                    model.Footer!.Logo = uniqueFileName;
                }

                if (model.Footer!.Id == 0)
                    _context.Footers.Add(model.Footer);
                else
                {
                    var existingFooter = _context.Footers.Find(model.Footer.Id);
                    if (existingFooter != null)
                    {
                        existingFooter.Description = model.Footer.Description;
                        existingFooter.Address = model.Footer.Address;
                        existingFooter.Email = model.Footer.Email;
                        existingFooter.FacebookUrl = model.Footer.FacebookUrl;
                        if (!string.IsNullOrEmpty(model.Footer.Logo))
                            existingFooter.Logo = model.Footer.Logo;
                    }
                }

                _context.SaveChanges();
                TempData["success"] = "Cập nhật thông tin web thành công!";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Có lỗi xảy ra khi cập nhật thông tin: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
