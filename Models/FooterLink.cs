using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class FooterLink
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Title { get; set; }

        public string? Url { get; set; }

        public int GroupId { get; set; } // 1: Thông tin, 2: Tài khoản, 3: Danh mục

        public int DisplayOrder { get; set; }

        public bool Status { get; set; }
    }
}
