using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGameConsole.Models.ViewModels
{
    public class FooterVM
    {
        public Footer? Footer { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? LogoFile { get; set; }

        public List<FooterLink>? InformationLinks { get; set; }
        public List<FooterLink>? AccountLinks { get; set; }
        public List<FooterLink>? CategoryLinks { get; set; }

    }
}
