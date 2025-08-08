using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Footer
    {
        [Key]
        public int Id { get; set; }

        public string? Logo { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Address { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }


        public string? FacebookUrl { get; set; }

        public bool Status { get; set; }

    }
}
