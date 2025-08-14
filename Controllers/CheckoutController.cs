using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using QuanLyGameConsole.Helper;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;
using QuanLyGameConsole.Services;

namespace QuanLyGameConsole.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly GameConsoleContext _context;
        private readonly PaypalClient _paypalClient;
        public List<CartRequest> Carts => CartHelper.GetCart(HttpContext.Session);

        public CheckoutController(GameConsoleContext context, PaypalClient paypalClient)
        {
            _context = context;
            _paypalClient = paypalClient;
        }
        public async Task<IActionResult> Index() // Thay đổi thành async Task<IActionResult>
        {
            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ShowLoginModal"] = true;
                return RedirectToAction("Index", "Home");
            }
            if (Carts == null || !Carts.Any())
            {
                TempData["error"] = "Giỏ hàng của bạn đang trống";
                return RedirectToAction("Cart", "Cart");
            }

            // Tạo một đối tượng CheckoutVM trống để bắt đầu
            var checkoutVM = new CheckoutVM();

            // --- BẮT ĐẦU PHẦN THÊM MỚI ---

            // 1. Lấy CustomerId của người dùng đang đăng nhập từ Claims
            var customerIdClaim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out var customerId))
            {
                // 2. Dùng CustomerId để truy vấn thông tin Customer từ database
                var customer = await _context.Customers.FindAsync(customerId);

                // 3. Nếu tìm thấy thông tin, điền vào đối tượng checkoutVM
                if (customer != null)
                {
                    checkoutVM.FullName = customer.FullName;
                    checkoutVM.Phone = customer.Phone;
                    checkoutVM.Email = customer.Email;
                    checkoutVM.Address = customer.Address;
                }
            }
            // --- KẾT THÚC PHẦN THÊM MỚI ---

            // 4. Tạo đối tượng cuối cùng để gửi đến View
            var checkoutValidationVM = new CheckoutValidationVM
            {
                CheckoutVM = checkoutVM, // Gửi checkoutVM đã được điền thông tin
                CartRequest = Carts
            };

            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(checkoutValidationVM);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutValidationVM checkoutValidationVM)
        {
            if (ModelState.IsValid)
            {
                var customerIdClaim = HttpContext.User.Claims.SingleOrDefault(c => c.Type == "CustomerId");
                if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out var customerId))
                {
                    var bill = new Bill
                    {
                        CustomerId = customerId,
                        FullName = checkoutValidationVM.CheckoutVM.FullName,
                        Phone = checkoutValidationVM.CheckoutVM.Phone,
                        Email = checkoutValidationVM.CheckoutVM.Email,
                        Address = checkoutValidationVM.CheckoutVM.Address,
                        /*Province = checkoutValidationVM.CheckoutVM.Province,
                        District = checkoutValidationVM.CheckoutVM.District,
                        Ward = checkoutValidationVM.CheckoutVM.Ward,*/
                        PaymentMethod = checkoutValidationVM.CheckoutVM.PaymentMethod,
                        Total = (decimal)Carts.Sum(item => item.Quantity * item.Price),
                        Status = 1,
                        OrderDate = DateTime.Now
                    };

                    await _context.Database.BeginTransactionAsync();

                    try
                    {
                        await _context.AddAsync(bill);
                        await _context.SaveChangesAsync();

                        var invoices = new List<Invoice>();
                        foreach (var item in Carts)
                        {

                            var productExists = await _context.Products.AnyAsync(p => p.ProductId == item.ProductId);
                            if (!productExists)
                            {
                                continue;
                            }
                            invoices.Add(new Invoice
                            {
                                BillId = bill.BillId,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                Price = (decimal)item.Price,
                                Total = (decimal)(item.Quantity * item.Price)
                            });
                        }
                        if (invoices.Any())
                        {
                            await _context.AddRangeAsync(invoices);
                            await _context.SaveChangesAsync();
                        }

                        await _context.Database.CommitTransactionAsync();

                        CartHelper.ClearCart(HttpContext.Session);
                        TempData["success"] = "Đã mua hàng thành công";
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        await _context.Database.RollbackTransactionAsync();
                    }
                }
            }
            return View("Index", checkoutValidationVM);
        }

        [HttpPost("/Checkout/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            // Thông tin đơn hàng gửi qua Paypal
            var TotalAmount = Carts.Sum(p => p.Total).ToString();
            var Currency = "USD";
            var OrderId = "DH" + DateTime.Now.Ticks.ToString();

            try
            {
                var response = await _paypalClient.CreateOrder(TotalAmount, Currency, OrderId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [HttpPost("/Checkout/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);
                return Ok(response);

            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

    }
}
