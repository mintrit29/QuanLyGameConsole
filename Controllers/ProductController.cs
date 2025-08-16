using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Controllers
{
    public class ProductController : Controller
    {

        private readonly GameConsoleContext _context;
        public ProductController(GameConsoleContext context)
        {

            _context = context;
        }

        public async Task<IActionResult> Index(string? search, string? categories = "", string? brands = "", double? minPrice = null, double? maxPrice = null, int page = 1, int? gender = null)
        {
            var pageSize = 5;  // Số sản phẩm mỗi trang

            // Bắt đầu câu truy vấn và áp dụng ngay bộ lọc trạng thái
            var products = _context.Products.Where(p => p.Status == 1);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                products = products.Where(p =>
                    p.ProductName.ToLower().Contains(search) ||
                    p.ShortDescription.ToLower().Contains(search));
            }

            // Lọc theo category
            if (!string.IsNullOrEmpty(categories))
            {
                products = products.Where(p => p.Category!.Slug == categories);
            }

            // Lọc theo brand
           

            // Lấy tổng số sản phẩm sau khi đã áp dụng TẤT CẢ bộ lọc
            var totalProducts = await products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Lấy các sản phẩm cho trang hiện tại
            var result = await products
                .Include(p => p.ProductRatings)
                .Skip((page - 1) * pageSize) // Bỏ qua các sản phẩm của các trang trước
                .Take(pageSize) // Lấy sản phẩm cho trang hiện tại
                .Select(p => new ProductVM
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName!,
                    Image = p.Image ?? "",
                    Price = p.Price,
                    ShortDescription = p.ShortDescription!,
                    ProductRating = p.ProductRatings.Any()
                        ? p.ProductRatings.Average(r => (double)r.Rating!) : 0,
                    Slug = p.Slug
                })
                .ToListAsync();

            // Tạo ViewModel cho phân trang
            var viewModel = new PagedProductListVM
            {
                Products = result,  // Danh sách sản phẩm cho trang hiện tại
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            // Trả về view với ViewModel
            return View(viewModel);
        }
        public async Task<IActionResult> SearchProduct(string? search = "", int page = 1)
        {
            var pageSize = 5;  // Số sản phẩm mỗi trang
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                products = products.Where(p =>
                    p.ProductName.ToLower().Contains(search) ||
                    p.ShortDescription.ToLower().Contains(search));
            }
            var totalProducts = await products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var result = await products
                .Where(p => p.Status == 1)
                .Include(p => p.ProductRatings)
                .Skip((page - 1) * pageSize) // Bỏ qua các sản phẩm của các trang trước
                .Take(pageSize) // Lấy sản phẩm cho trang hiện tại
                .Select(p => new ProductVM
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName!,
                    Image = p.Image ?? "",
                    Price = p.Price,
                    ShortDescription = p.ShortDescription!,
                    ProductRating = p.ProductRatings.Any()
                        ? p.ProductRatings.Average(r => (double)r.Rating!) : 0,
                    TotalRating = p.ProductRatings.Count,
                }).ToListAsync();
            var viewModel = new PagedProductListVM
            {
                Products = result,  // Danh sách sản phẩm cho trang hiện tại
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };
            return View(viewModel);
        }
        [Route("ProductDetail/{slug}")]
        public async Task<IActionResult> ProductDetail(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            var customerId = customerIdClaim != null ? int.Parse(customerIdClaim.Value) : (int?)null;
            // Lấy sản phẩm, đánh giá, và bình luận từ cơ sở dữ liệu
            var product = await _context.Products
                .Include(p => p.Category)
              
                .Include(p => p.ProductImages)
                .Include(p => p.ProductComments).ThenInclude(productComment => productComment.Customer)
                .Include(p => p.ProductRatings)
                .ThenInclude(c => c.Customer)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (product == null) // Kiểm tra sản phẩm tồn tại
            {
                return NotFound();
            }

            var relatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId || p.BrandId == product.BrandId && p.ProductId != product.ProductId)
                .Take(5)
                .ToListAsync();
            // Tạo ViewModel
            var viewModel = new ProductDetailVM
            {

                Product = product,
                RelatedProducts = relatedProducts,
                ProductRating = product.ProductRatings.Any()
                    ? product.ProductRatings.Average(r => (double)r.Rating!) // Tính trung bình điểm đánh giá
                    : 0,
                TotalRating = product.ProductRatings.Count, // Tổng số đánh giá
                Comments = product.ProductComments
                    .Select(c => new ProductCommentVM
                    {
                        CustomerName = c.Customer?.DisplayName ?? "Guest", // Hiển thị tên khách
                        Content = c.Contents,
                        CreatedAt = c.CreatedAt,
                        Rating = product.ProductRatings.FirstOrDefault(r => r.CustomerId == c.CustomerId)?.Rating,
                        Reply = c.Reply //
                    }).ToList(),
            };

            return View(viewModel); // Trả về View
        }
        [HttpPost]
        [Route("ProductDetail/{slug}/AddReview")] // Thay đổi route
        public IActionResult AddReview(string slug, string content, int rating) // Thay đổi tham số
        {
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            int? customerId = customerIdClaim != null ? int.Parse(customerIdClaim.Value) : (int?)null;

            // Thay đổi ở đây: Tìm sản phẩm bằng slug thay vì id
            var product = _context.Products.FirstOrDefault(p => p.Slug == slug);
            if (product == null)
            {
                return NotFound();
            }

            // Quan trọng: Vẫn sử dụng product.ProductId (là số) để lưu vào CSDL
            var comment = new ProductComment
            {
                ProductId = product.ProductId, // Dùng ID lấy từ sản phẩm đã tìm được
                CustomerId = customerId,
                Contents = content,
                CreatedAt = DateTime.Now
            };

            var productRating = new ProductRating
            {
                ProductId = product.ProductId, // Dùng ID lấy từ sản phẩm đã tìm được
                CustomerId = customerId,
                Rating = rating
            };

            _context.ProductComments.Add(comment);
            _context.ProductRatings.Add(productRating);
            _context.SaveChanges();

            // Thay đổi ở đây: Redirect lại trang chi tiết sản phẩm bằng slug
            return RedirectToAction("ProductDetail", new { slug = product.Slug });
        }


    }
}