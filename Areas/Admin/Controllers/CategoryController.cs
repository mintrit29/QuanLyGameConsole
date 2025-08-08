using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Helper;
using QuanLyGameConsole.Models;



namespace DongHo_Admin.Areas.Admin.Controllers

{
    [Area("Admin")]

    [Authorize(Policy = "Admin")]

    public class CategoryController : Controller

    {

        private readonly GameConsoleContext _context;

        public CategoryController(GameConsoleContext context)

        {



            _context = context;

        }

        public async Task<IActionResult> Index()

        {

            var categories = await _context.Categories.ToListAsync();

            return View(categories);

        }

        // Thêm danh mục

        [HttpPost]

        public async Task<IActionResult> Create(Category model)

        {

            // Log model để kiểm tra dữ liệu gửi từ client

            Console.WriteLine($"CategoryName: {model.CategoryName}, ParentId: {model.ParentId}, Slug: {model.Slug}");



            if (ModelState.IsValid)

            {

                model.Slug = await SlugHelper.GenerateUniqueSlug(_context, model.CategoryName!, SlugHelper.EntityType.Category, model.CategoryId);

                _context.Categories.Add(model);
                try

                {

                    await _context.SaveChangesAsync(); // Dùng async để không bị block thread

                    return Json(new { success = true });

                }

                catch (DbUpdateException ex)

                {

                    // Log lỗi và trả về phản hồi lỗi

                    return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });

                }

                catch (Exception ex)

                {

                    // Lỗi chung

                    return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });

                }

            }

            else

            {

                // Log thông báo nếu ModelState không hợp lệ

                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

            }

        }

        // Cập nhật danh mục

        [HttpPost]

        public async Task<IActionResult> Edit(Category model)

        {

            if (ModelState.IsValid)

            {

                var category = await _context.Categories.FindAsync(model.CategoryId);

                if (category != null)

                {

                    category.CategoryName = model.CategoryName;

                    category.Slug = await SlugHelper.GenerateUniqueSlug(_context, category.CategoryName!, SlugHelper.EntityType.Category, model.CategoryId);

                    category.ParentId = model.ParentId;



                    _context.Update(category);

                    await _context.SaveChangesAsync();



                    return Json(new { success = true, message = "Cập nhật danh mục thành công!" });

                }

                return Json(new { success = false, message = "Không tìm thấy danh mục!" });

            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

        }



        [HttpPost]

        public IActionResult Delete(int id)

        {

            var category = _context.Categories.Find(id);

            if (category != null)

            {

                _context.Categories.Remove(category);

                _context.SaveChanges();



                return Json(new { success = true, message = "Xóa danh mục thành công!" });

            }

            return Json(new { success = false, message = "Không tìm thấy danh mục!" });

        }
        [HttpGet]
        public IActionResult Search(string searchQuery)
        {
            var categories = _context.Categories
                                    .Where(c => c.CategoryName!.ToLower().Contains(searchQuery) || c.Slug!.Contains(searchQuery))
                                    .ToList();

            return Json(new { success = true, data = categories });
        }





    }

}