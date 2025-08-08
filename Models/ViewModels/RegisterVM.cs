using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Tên đăng nhập không được trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Tên đăng nhập có ít nhất 6 ký tự")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Tên đăng nhập chỉ chứa chữ không dấu và số")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được trống")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ thường, chữ hoa, số và ký tự đặc biệt")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu không được trống")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ thường, chữ hoa, số và ký tự đặc biệt")]
        public string? ConfirmPassword { get; set; }
    }
}
