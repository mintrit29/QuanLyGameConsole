using Microsoft.AspNetCore.Mvc;
using QuanLyGameConsole.Models;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = CartHelper.GetCart(HttpContext.Session);
            return View("Default", new CartVM
            {
                Quantity = cart.Sum(p => p.Quantity),
                Total = cart.Sum(p => p.Total)
            });
        }
    }
}
