using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Controllers
{
    public class AccountController : Controller
    {

        private readonly GameConsoleContext _context;
        public AccountController(GameConsoleContext context)
        {

            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (customerIdClaim == null) return RedirectToAction("Index", "Home");

            int customerId = int.Parse(customerIdClaim.Value);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.AccountId == customerId);

            if (customer == null) return NotFound();
            return View(customer);
        }
        public async Task<IActionResult> Edit()
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (customerIdClaim == null) return RedirectToAction("Index", "Home");

            int customerId = int.Parse(customerIdClaim.Value);

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.AccountId == customerId);

            if (customer == null) return NotFound();

            // Tạo CustomerVM và truyền dữ liệu vào từ khách hàng
            var customerVM = new CustomerVM
            {
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                Email = customer.Email,
                DisplayName = customer.DisplayName,
                Dob = customer.Dob,
                Gender = customer.Gender
            };

            return View(customerVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerVM customerVM)
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (customerIdClaim == null) return RedirectToAction("Index", "Home");

            int customerId = int.Parse(customerIdClaim.Value);

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.AccountId == customerId);

            if (customer == null) return NotFound();

            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ, trả về lại form để hiển thị lỗi
                return View(customerVM);
            }

            customer.FullName = customerVM.FullName;
            customer.Phone = customerVM.Phone;
            customer.Address = customerVM.Address;
            customer.Email = customerVM.Email;
            customer.DisplayName = customerVM.DisplayName;
            customer.Dob = customerVM.Dob;
            customer.Gender = customerVM.Gender;

            _context.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> Order(int? status)
        {
            ViewBag.Title = "Đơn hàng";

            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim == null) return RedirectToAction("Index", "Home");

            int customerId = int.Parse(customerIdClaim.Value);
            Console.WriteLine($"Customer ID: {customerId}");

            var query = _context.Bills.Where(b => b.CustomerId == customerId);

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            var bills = await query.ToListAsync();

            return View(bills);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var bill = await _context.Bills.FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null)
            {
                return NotFound();
            }

            if (bill.Status == 2) // Giữ lại logic không cho hủy đơn hàng đã thanh toán
            {
                TempData["error"] = "Đơn hàng đã thanh toán, không thể xóa.";
                return RedirectToAction("Order");
            }

            // Xóa đơn hàng khỏi database
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            TempData["success"] = "Đơn hàng đã được xóa thành công.";

            return RedirectToAction("Order");
        }


        public IActionResult Favorite()
        {
            //int? customerId = HttpContext.Session.GetInt32("CustomerId");
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            int customerId = int.Parse(customerIdClaim!.Value);
            var favoriteProducts = _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.CustomerId == customerId)
                .Select(f => new FavoriteVM
                {
                    ProductId = f.Product.ProductId,
                    Name = f.Product.ProductName!,
                    Price = f.Product.Price,
                    Image = f.Product.Image,
                    Slug = f.Product.Slug!
                }).ToList();

            return View(favoriteProducts);
        }
        [HttpPost]
        public JsonResult AddToWishlist(int productId)
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm vào danh sách yêu thích." });
            }
            int customerId = int.Parse(customerIdClaim.Value);
            var existingWishlist = _context.Favorites
                .FirstOrDefault(w => w.CustomerId == customerId && w.ProductId == productId);

            if (existingWishlist != null)
            {
                return Json(new { success = false, message = "Sản phẩm đã có trong danh sách yêu thích!" });
            }
            // Thêm sản phẩm mới vào danh sách yêu thích
            var wishlist = new Favorite
            {
                CustomerId = customerId,
                ProductId = productId
            };
            _context.Favorites.Add(wishlist);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã thêm vào danh sách yêu thích!" });
        }
        [HttpPost] // Chỉ cho phép truy cập bằng phương thức POST từ JavaScript
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            // Lấy ID của khách hàng đang đăng nhập
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện hành động này." });
            }
            int customerId = int.Parse(customerIdClaim.Value);

            // Tìm sản phẩm yêu thích cụ thể của khách hàng này
            var favoriteItem = await _context.Favorites
                .FirstOrDefaultAsync(f => f.CustomerId == customerId && f.ProductId == productId);

            if (favoriteItem == null)
            {
                // Nếu không tìm thấy, trả về lỗi
                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong danh sách yêu thích." });
            }

            // Nếu tìm thấy, tiến hành xóa
            _context.Favorites.Remove(favoriteItem);
            await _context.SaveChangesAsync();

            // Trả về thông báo thành công
            return Json(new { success = true, message = "Đã xóa sản phẩm khỏi danh sách yêu thích." });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }



        [HttpPost] // <-- Đảm bảo có dòng này
        [ValidateAntiForgeryToken] // <-- Đảm bảo có dòng này
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            // Lấy CustomerId từ claims để đảm bảo bảo mật
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out var customerId))
            {
                // Trả về lỗi 401 Unauthorized nếu không xác thực được
                return StatusCode(401, new { success = false, message = "Lỗi xác thực người dùng." });
            }

            // Tìm đơn hàng theo BillId VÀ CustomerId của người dùng đang đăng nhập
            var bill = await _context.Bills
                .FirstOrDefaultAsync(b => b.BillId == id && b.CustomerId == customerId);

            if (bill == null)
            {
                // Trả về lỗi 404 Not Found nếu không tìm thấy đơn hàng
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });
            }

            // Chỉ cho phép xác nhận nếu đơn hàng đang ở trạng thái "Chưa thanh toán" (Status = 1)
            if (bill.Status != 1)
            {
                // Trả về lỗi 400 Bad Request nếu trạng thái không hợp lệ
                return BadRequest(new { success = false, message = "Đơn hàng không ở trạng thái có thể xác nhận." });
            }

            try
            {
                // Cập nhật trạng thái thành "Đã thanh toán" (Status = 2)
                bill.Status = 2;
                _context.Update(bill);
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return Json(new { success = true, message = "Xác nhận thanh toán thành công." });
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi lưu vào DB, trả về lỗi 500 Internal Server Error
                // Ghi lại lỗi để gỡ lỗi (bạn có thể dùng ILogger ở đây)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi phía máy chủ khi cập nhật đơn hàng." });
            }
        }
    }
}