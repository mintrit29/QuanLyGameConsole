using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace QuanLyGameConsole.Models
{
    public partial class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public int BillId { get; set; }

        public int ProductId { get; set; }

        [Precision(18, 0)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Precision(18, 0)]
        public decimal Total { get; set; }

        public virtual Bill Bill { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
