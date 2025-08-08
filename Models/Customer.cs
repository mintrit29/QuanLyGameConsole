using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? FullName { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string? Phone { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Address { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? Email { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? DisplayName { get; set; }
        public DateOnly? Dob { get; set; }

        public bool? Gender { get; set; }

        public int? AccountId { get; set; }

        public virtual Account? Account { get; set; }

        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public virtual ICollection<ProductComment> ProductComments { get; set; } = new List<ProductComment>();

        public virtual ICollection<ProductRating> ProductRatings { get; set; } = new List<ProductRating>();
    }
}
