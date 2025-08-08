using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Slug { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
