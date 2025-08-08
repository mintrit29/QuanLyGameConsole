using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models
{
    public partial class ProductRating
    {
        [Key]
        public int RatingId { get; set; }

        public int ProductId { get; set; }

        public int? CustomerId { get; set; }

        public int? Rating { get; set; }

        public virtual Customer? Customer { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
