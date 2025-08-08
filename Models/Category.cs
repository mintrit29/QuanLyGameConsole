using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? CategoryName { get; set; }

        public int? ParentId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Slug { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
