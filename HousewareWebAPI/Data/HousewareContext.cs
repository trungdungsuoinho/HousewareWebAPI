using HousewareWebAPI.Data.Entities;
using HousewareWebAPI.Helpers.Common;
using Microsoft.EntityFrameworkCore;

namespace Houseware.WebAPI.Data
{
    public class HousewareContext : DbContext
    {
        public HousewareContext(DbContextOptions<HousewareContext> options) : base(options)
        {
        }

        public DbSet<Classification> Classifications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Stored> Storeds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Classification
            modelBuilder.Entity<Classification>().Property(c => c.Sort).HasDefaultValue(int.MaxValue);
            modelBuilder.Entity<Classification>().Property(c => c.Enable).HasDefaultValue(false);

            // Category
            modelBuilder.Entity<Category>().Property(c => c.Sort).HasDefaultValue(int.MaxValue);
            modelBuilder.Entity<Category>().Property(c => c.Enable).HasDefaultValue(false);

            // Product
            modelBuilder.Entity<Product>().Property(p => p.Sort).HasDefaultValue(int.MaxValue);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.Weight).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.Length).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.Width).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.Height).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.View).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.CreateDate).HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
            modelBuilder.Entity<Product>().Property(p => p.ModifyDate).HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
            modelBuilder.Entity<Product>().Property(p => p.Enable).HasDefaultValue(false);

            // Specification
            modelBuilder.Entity<Specification>().Property(p => p.Sort).HasDefaultValue(int.MaxValue);

            // ProductSpecification
            modelBuilder.Entity<ProductSpecification>().HasKey(ps => new { ps.ProductId, ps.SpecificationId });

            // Customer
            modelBuilder.Entity<Customer>().HasIndex(u => u.Phone).IsUnique(true);
            modelBuilder.Entity<Customer>().Property(u => u.VerifyPhone).HasDefaultValue("N");
            modelBuilder.Entity<Customer>().HasIndex(u => u.Email).IsUnique(true);
            modelBuilder.Entity<Customer>().Property(u => u.VerifyEmail).HasDefaultValue("N");
            modelBuilder.Entity<Customer>().Property(p => p.CreateDate).HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
            modelBuilder.Entity<Customer>().HasOne(c => c.DefaultAddress).WithOne(a => a.DefaultCustomer).HasForeignKey<Customer>(c => c.DefaultAddressId);

            // Cart
            modelBuilder.Entity<Cart>().HasKey(c => new { c.CustomerId, c.ProductId });

            // Address
            modelBuilder.Entity<Address>().Property(a => a.ModifyDate).HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
            modelBuilder.Entity<Address>().HasOne(a => a.Customer).WithMany(c => c.Addresses).HasForeignKey(a => a.CustomerId);

            // Store

            // Stored
            modelBuilder.Entity<Stored>().HasKey(s => new { s.StoreId, s.ProductId });

            // Order
            modelBuilder.Entity<Order>().Property(p => p.OrderDate).HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");
            modelBuilder.Entity<Order>().Property(p => p.PaymentType).HasDefaultValue(GlobalVariable.PayCod);
            modelBuilder.Entity<Order>().Property(p => p.StatusPaid).HasDefaultValue(false);
            modelBuilder.Entity<Order>().HasOne(o => o.Address).WithMany(a => a.Orders).OnDelete(DeleteBehavior.SetNull);

            // OrderDetail
            modelBuilder.Entity<OrderDetail>().HasKey(o => new { o.OrderId, o.ProductId });

        }
    }
}
