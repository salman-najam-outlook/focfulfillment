using LocalDropshipping.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Data
{
    public class LocalDropshippingContext : DbContext
    {
        public LocalDropshippingContext(DbContextOptions<LocalDropshippingContext> options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Withdrawals> Withdrawals { get; set; }
    }
}
