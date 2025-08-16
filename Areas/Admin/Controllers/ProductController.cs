using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;

namespace QuanLyGameConsole.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class ProductController : Controller
    {
        private readonly GameConsoleContext _context;

        public ProductController(GameConsoleContext context)
        {
            _context = context;
        }

        // GET: Danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
     .Include(p => p.Category) // Removed the 'Brand' include
     .OrderByDescending(p => p.ProductId)
     .ToListAsync();
            return View(products);
        }

        // GET: Form thêm sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Brands = _context.Brands
                .Select(b => new SelectListItem
                {
                    Value = b.BrandId.ToString(),
               
                }).ToList();

            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                // Upload ảnh
                if (Image != null && Image.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Path.GetFileName(Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var file = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(file);
                    }

                    product.Image = "" + fileName;
                }
                else
                {
                    product.Image = "/images/no-image.png";
                }

                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                product.Status = 1;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Load lại dropdown nếu có lỗi
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Brands = _context.Brands
                .Select(b => new SelectListItem
                {
                    Value = b.BrandId.ToString(),
                   
                }).ToList();

            TempData["error"] = "Vui lòng nhập đầy đủ thông tin!";
            return View(product);
        }

        // GET: Form chỉnh sửa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories
     .Select(c => new SelectListItem
     {
         Value = c.CategoryId.ToString(),
         Text = c.CategoryName
     }).ToList();

            return View(product);
        }

        // POST: Xử lý chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? Image)
        {
            if (id != product.ProductId)
            {
                TempData["error"] = "ID sản phẩm không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == id);
                if (existingProduct == null)
                {
                    TempData["error"] = "Không tìm thấy sản phẩm";
                    return RedirectToAction(nameof(Index));
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Nếu có upload ảnh mới
                if (Image != null && Image.Length > 0)
                {
                    // Xóa ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(existingProduct.Image))
                    {
                        var oldImagePath = Path.Combine(uploadsFolder, existingProduct.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Tạo tên file mới duy nhất
                    string fileExt = Path.GetExtension(Image.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileExt;
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Lưu ảnh mới
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    product.Image = fileName;
                }
                else
                {
                    // Giữ lại ảnh cũ nếu không upload
                    product.Image = existingProduct.Image;
                }

                product.UpdatedAt = DateTime.Now;
                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }


        // Xóa sản phẩm + dữ liệu liên quan
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["error"] = "Không tìm thấy sản phẩm";
                    return RedirectToAction(nameof(Index));
                }

                _context.ProductComments.RemoveRange(_context.ProductComments.Where(c => c.ProductId == id));
                _context.ProductRatings.RemoveRange(_context.ProductRatings.Where(r => r.ProductId == id));
                _context.ProductImages.RemoveRange(_context.ProductImages.Where(i => i.ProductId == id));
                _context.Favorites.RemoveRange(_context.Favorites.Where(f => f.ProductId == id));
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
                TempData["success"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Xóa sản phẩm thất bại: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // Ẩn sản phẩm
        [HttpPost]
        public async Task<IActionResult> HideProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Status = 0;
                product.UpdatedAt = DateTime.Now;
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["success"] = "Sản phẩm đã bị ẩn!";
            }
            else
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
            }
            return RedirectToAction(nameof(Index));
        }

        // Hiện sản phẩm
        [HttpPost]
        public async Task<IActionResult> ShowProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Status = 1;
                product.UpdatedAt = DateTime.Now;
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["success"] = "Sản phẩm đã hiển thị lại!";
            }
            else
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
