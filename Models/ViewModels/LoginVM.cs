using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Tên đăng nhập không được trống")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được trống")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
