using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly GameConsoleContext _context;
        public AccountController(GameConsoleContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == loginVM.Username);
            if (account == null)
            {
                ModelState.AddModelError("Username", "Tài khoản Admin không tồn tại");
                return View(loginVM);
            }

            if (account.Password != loginVM.Password)
            {
                ModelState.AddModelError("Password", "Tài khoản hoặc mật khẩu không đúng");
                return View(loginVM);
            }

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(loginVM.Username))
            {
                claims.Add(new Claim(ClaimTypes.Name, loginVM.Username));
            }
            if (account.RoleId > 0)
            {
                var roleValue = account.RoleId.ToString();
                if (!string.IsNullOrEmpty(roleValue))
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleValue));
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Admin");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = returnUrl
            };

            await HttpContext.SignInAsync("Admin", new ClaimsPrincipal(claimsIdentity), authProperties);

            if (account.RoleId == 2)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Admin");
            return RedirectToAction("Login", "Account");
        }

    }
}
