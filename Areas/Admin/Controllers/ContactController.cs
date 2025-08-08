using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;

namespace QuanLyGameConsole.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class ContactController : Controller
    {
        private readonly GameConsoleContext _context;
        public ContactController(GameConsoleContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.ToListAsync();
            return View(contacts);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, int status)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return Json(new { success = false, message = "Không tìm thấy liên hệ" });
            }

            contact.Status = status;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật thành công" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                TempData["error"] = "Không tìm thấy liên hệ";
                return RedirectToAction("Index");
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            TempData["success"] = "Xoá thành công";
            return RedirectToAction("Index");
        }
    }
}