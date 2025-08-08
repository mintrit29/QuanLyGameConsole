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

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // Hiển thị form cập nhật sản phẩm
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            // Trả về view cập nhật sản phẩm
            return View(product);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.ProductId)
            {
                TempData["error"] = "ID sản phẩm không hợp lệ";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và xử lý file ảnh (nếu có)
                    if (imageFile != null)
                    {
                        string fileName = Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        product.Image = "/images/" + fileName; // Lưu đường dẫn ảnh vào cơ sở dữ liệu
                    }

                    // Cập nhật thông tin sản phẩm
                    product.UpdatedAt = DateTime.Now;

                    // Kiểm tra trạng thái của sản phẩm trước khi lưu
                    _context.Update(product); // Cập nhật sản phẩm
                    var result = await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                    if (result > 0) // Kiểm tra xem có dòng nào bị thay đổi trong DB không
                    {
                        TempData["success"] = "Cập nhật sản phẩm thành công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Không có thay đổi nào được lưu vào cơ sở dữ liệu.";
                        return View(product); // Trả lại view nếu không có thay đổi nào
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["error"] = $"Lỗi cập nhật dữ liệu: {ex.Message}";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }

            // Nếu ModelState không hợp lệ, trả về view với thông báo lỗi
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> HideProduct(int id)
        {
            // Tìm sản phẩm theo ID
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            // Đánh dấu sản phẩm là đã ẩn (bằng cách đặt Deleted = 1 hoặc Status = 0)
            product.Status = 0; // Hoặc sử dụng product.Deleted = 1;

            // Cập nhật thông tin sản phẩm
            product.UpdatedAt = DateTime.Now;
            _context.Update(product);

            await _context.SaveChangesAsync();

            TempData["success"] = "Sản phẩm đã bị ẩn!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ShowProduct(int id)
        {
            // Tìm sản phẩm theo ID
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            // Đánh dấu sản phẩm là hiển thị lại (thay đổi Status hoặc Deleted)
            product.Status = 1; // Hoặc product.Deleted = 0;

            // Cập nhật thông tin sản phẩm
            product.UpdatedAt = DateTime.Now;
            _context.Update(product);

            await _context.SaveChangesAsync();

            TempData["success"] = "Sản phẩm đã hiển thị lại!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm sản phẩm theo ID
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            try
            {
                // Xóa sản phẩm khỏi cơ sở dữ liệu
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["success"] = "Sản phẩm đã được xóa!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Lỗi khi xóa sản phẩm: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}