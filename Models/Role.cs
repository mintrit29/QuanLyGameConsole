using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Type { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
