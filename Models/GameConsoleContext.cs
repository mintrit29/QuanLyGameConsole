using Microsoft.EntityFrameworkCore;

namespace QuanLyGameConsole.Models
{
    public partial class GameConsoleContext : DbContext
    {
        public GameConsoleContext(DbContextOptions<GameConsoleContext> options) : base(options)
        {
        }

        public required virtual DbSet<Account> Accounts { get; set; }

        public required virtual DbSet<Bill> Bills { get; set; }

        public required virtual DbSet<Brand> Brands { get; set; }

        public required virtual DbSet<Category> Categories { get; set; }

        public required virtual DbSet<Contact> Contacts { get; set; }

        public required virtual DbSet<Customer> Customers { get; set; }

        public required virtual DbSet<Favorite> Favorites { get; set; }

        public required virtual DbSet<Invoice> Invoices { get; set; }

        public required virtual DbSet<Product> Products { get; set; }

        public required virtual DbSet<ProductComment> ProductComments { get; set; }

        public required virtual DbSet<ProductImage> ProductImages { get; set; }

        public required virtual DbSet<ProductRating> ProductRatings { get; set; }

        public required virtual DbSet<Role> Roles { get; set; }

        public required virtual DbSet<Footer> Footers { get; set; }

        public required virtual DbSet<FooterLink> FooterLinks { get; set; }

        public required virtual DbSet<About> Abouts { get; set; }

        public required virtual DbSet<Policy> Policies { get; set; }

        public required virtual DbSet<Slider> Sliders { get; set; }
    }
}
