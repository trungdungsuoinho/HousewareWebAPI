﻿using Houseware.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace Houseware.WebAPI.Data
{
    public class HousewareContext : DbContext
    {
        public HousewareContext(DbContextOptions<HousewareContext> options) : base(options)
        {
        }

        public DbSet<Classification> Classifications { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Classification
            modelBuilder.Entity<Classification>().Property(c => c.Enable).HasDefaultValue(false);
            modelBuilder.Entity<Classification>().Property(c => c.Sort).HasDefaultValue(int.MaxValue);


            //// User
            //modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(true);
            //modelBuilder.Entity<User>().Property(u => u.VerifyPhone).HasDefaultValue(false);
            //modelBuilder.Entity<User>().Property(u => u.VerifyEmail).HasDefaultValue(false);


            //// Category
            //modelBuilder.Entity<Category>().Property(c => c.Enable).HasDefaultValue(false);

            //// Product
            //modelBuilder.Entity<Product>().Property(c => c.Enable).HasDefaultValue(false);
        }
    }
}