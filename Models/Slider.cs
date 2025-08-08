using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models
{
    public partial class Slider
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Title { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; } = true;


    }
}
