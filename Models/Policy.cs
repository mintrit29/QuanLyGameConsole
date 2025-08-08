using System.ComponentModel.DataAnnotations;

namespace QuanLyGameConsole.Models
{
    public partial class Policy
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}