using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "Họ Tên là bắt buộc.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Nội dung là bắt buộc.")]
        [StringLength(500, ErrorMessage = "Nội dung nhập giới hạn trong 500 ký tự")]
        public string? Note { get; set; }
    }
}
