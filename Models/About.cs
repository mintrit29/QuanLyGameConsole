using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models
{
    public partial class About
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
