using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Username { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Password { get; set; }

        public string? Email { get; set; }
        public int? RoleId { get; set; }

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

        public virtual Role? Role { get; set; }
    }
}
