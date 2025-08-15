using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class ProductComment
    {
        [Key]
        public int CommentId { get; set; }

        public int ProductId { get; set; }

        public int? CustomerId { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Contents { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? Reply { get; set; }
        public DateTime? ReplyAt { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
