using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models.ViewModels
{
    public class CheckoutValidationVM
    {
        public List<CartRequest> CartRequest { get; set; } = new();
        public CheckoutVM CheckoutVM { get; set; } = null!;
    }
}
