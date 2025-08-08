using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DongHo_Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
