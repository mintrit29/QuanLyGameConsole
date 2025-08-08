using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace DongHo_Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class StatisticalController : Controller
    {
        private readonly GameConsoleContext _context;

        public StatisticalController(GameConsoleContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("GetRevenue")]
        public IActionResult GetRevenue()
        {
            var chartData = _context.Bills
                .Join(_context.Invoices,
                    b => b.BillId,
                    i => i.BillId,
                    (b, i) => new RevenueStatisticVM
                    {
                        Date = b.OrderDate.Date,
                        Revenue = i.Quantity * i.Price
                    })
                .GroupBy(s => s.Date)
                .Select(group => new RevenueStatisticVM
                {
                    Date = group.Key,
                    Revenue = group.Sum(s => s.Revenue)
                })
                .OrderBy(s => s.Date)
                .ToList();

            var result = chartData.Select(item => new
            {
                Date = item.Date.ToString("yyyy-MM-dd"),
                Revenue = item.Revenue
            }).ToList();

            return Json(result);
        }
        [HttpPost]
        [Route("GetPurchase")]
        //public IActionResult GetPurchase()
        //{
        //    var purchaseData = _context.Bills
        //        .Join(_context.Invoices,
        //            b => b.BillId,
        //            i => i.BillId,
        //            (b, i) => new
        //            {
        //                Date = b.OrderDate.Date,
        //                Quantity = i.Quantity
        //            })
        //        .GroupBy(s => s.Date)
        //        .Select(group => new
        //        {
        //            date = group.Key.ToString("yyyy-MM-dd"),
        //            TotalPurchases = group.Sum(s => s.Quantity)
        //        })
        //        .OrderBy(s => s.date)
        //        .ToList();

        //    return Json(purchaseData);
        //}
        [HttpPost]
        [Route("SubmitFilterDate")]
        public IActionResult SubmitFilterDate(string filterdate)
        {
            // Kiểm tra và chuyển đổi ngày tháng
            if (!DateTime.TryParse(filterdate, out var parsedDate))
            {
                return BadRequest("Ngày không hợp lệ.");
            }

            try
            {
                // Truy vấn với so sánh DateTime trực tiếp
                var chartData = _context.Bills
                    .Where(b => b.OrderDate.Date == parsedDate.Date) // So sánh trực tiếp
                    .Join(_context.Invoices,
                        b => b.BillId,
                        i => i.BillId,
                        (b, i) => new RevenueStatisticVM
                        {
                            Date = b.OrderDate,
                            Revenue = i.Quantity * i.Price, // Tính doanh thu
                        })
                    .GroupBy(s => s.Date)
                    .Select(group => new RevenueStatisticVM
                    {
                        Date = group.Key,
                        Revenue = group.Sum(s => s.Revenue),
                    })
                    .ToList();

                return Json(chartData);
            }
            catch (Exception ex)
            {
                // Log lỗi
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý.");
            }
        }
    }
}
