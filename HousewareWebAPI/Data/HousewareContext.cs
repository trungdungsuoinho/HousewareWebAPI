using HousewareWebAPI.Data.Entities;
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
        //public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Classification
            modelBuilder.Entity<Classification>().Property(c => c.Sort).HasDefaultValue(int.MaxValue);
            modelBuilder.Entity<Classification>().Property(c => c.Enable).HasDefaultValue(false);

            // Category
            modelBuilder.Entity<Category>().Property(c => c.Sort).HasDefaultValue(int.MaxValue);
            modelBuilder.Entity<Category>().Property(c => c.Enable).HasDefaultValue(false);
            modelBuilder.Entity<Category>().HasOne(c => c.Classification).WithMany(c => c.Categories).OnDelete(DeleteBehavior.SetNull);

            // Product
            modelBuilder.Entity<Product>().Property(p => p.Sort).HasDefaultValue(int.MaxValue);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.View).HasDefaultValue(0);
            modelBuilder.Entity<Product>().Property(p => p.CreateDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Product>().Property(p => p.ModifyDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Product>().Property(p => p.Enable).HasDefaultValue(false);
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(p => p.Products).OnDelete(DeleteBehavior.SetNull);


            //// User
            //modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(true);
            //modelBuilder.Entity<User>().Property(u => u.VerifyPhone).HasDefaultValue(false);
            //modelBuilder.Entity<User>().Property(u => u.VerifyEmail).HasDefaultValue(false);
        }
    }
}
