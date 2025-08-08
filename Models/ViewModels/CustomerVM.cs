using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models.ViewModels
{
    public class CustomerVM
    {
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng.")]
        public string? FullName { get; set; }
        [StringLength(10)]
        [RegularExpression(@"^(0[3|5|7|8|9])\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ. Vui lòng nhập đúng số điện thoại")]
        public string? Phone { get; set; }
        [StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự.")]
        public string? Address { get; set; }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Tên hiển thị không được trống")]
        public string? DisplayName { get; set; }
        public DateOnly? Dob { get; set; }
        public bool? Gender { get; set; }

    }
}
