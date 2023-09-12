using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Data
{
    public class LocalDropshippingContext : IdentityDbContext<User>
    {
        public LocalDropshippingContext(DbContextOptions<LocalDropshippingContext> options) : base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Withdrawals> Withdrawals { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
    }
}
