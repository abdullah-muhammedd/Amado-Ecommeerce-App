using Amado.Models;
using Microsoft.EntityFrameworkCore;

namespace Amado.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart_Item>().HasKey(ci => new { ci.CartID, ci.ItemID });

            modelBuilder.Entity<Cart_Item>().HasOne(c => c.Cart).WithMany(ci => ci.CartItems).HasForeignKey(c => c.CartID);
            modelBuilder.Entity<Cart_Item>().HasOne(i => i.Item).WithMany(ci => ci.CartItems).HasForeignKey(i => i.ItemID);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Cart_Item> Cart_Item { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Order_Item> Order_Item { get; set; }
    }
}
