using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; } = null!;
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = null!;
        [Column(TypeName = "varchar(200)")]
        public string Subject { get; set; } = null!;
        [Column(TypeName = "nvarchar(500)")]
        public string Note { get; set; } = null!;

        public int Status { get; set; } = 0;
    }
}
