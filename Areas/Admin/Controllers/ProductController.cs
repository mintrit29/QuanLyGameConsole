using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                TempData["error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["success"] = "Xóa sản phẩm thành công.";
            }
            catch
            {
                TempData["error"] = "Xóa sản phẩm thất bại.";
            }

            return RedirectToAction("Index");
        }

        // GET: Hiển thị form thêm sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product());
        }

        // POST: Xử lý thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                // Nếu có upload ảnh
                if (Image != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Path.GetFileName(Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var file = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(file);
                    }

                    product.Image = "/images/" + fileName;
                }
                else
                {
                    // Nếu không upload ảnh, set ảnh mặc định
                    product.Image = "/images/no-image.png";
                }

                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                product.Status = 1; // Mặc định hiển thị

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Vui lòng nhập đầy đủ thông tin!";
            return View(product);
        }

        // GET: Danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();
            return View(products);
        }

        // GET: Hiển thị form chỉnh sửa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.ProductId)
            {
                TempData["error"] = "ID sản phẩm không hợp lệ";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                if (existingProduct == null)
                {
                    TempData["error"] = "Không tìm thấy sản phẩm";
                    return RedirectToAction(nameof(Index));
                }

                // Nếu upload ảnh mới
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.Image = "/images/" + fileName;
                }
                else
                {
                    product.Image = existingProduct.Image; // Giữ ảnh cũ
                }

                product.UpdatedAt = DateTime.Now;
                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Dữ liệu không hợp lệ!";
            return View(product);
        }

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

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Sản phẩm đã được xóa!";
            }
            else
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
