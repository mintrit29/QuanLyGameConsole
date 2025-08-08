using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }

        public int? ProductId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Image { get; set; }

        public virtual Product? Product { get; set; }
    }
}
