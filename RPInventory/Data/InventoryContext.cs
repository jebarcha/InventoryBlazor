﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RPInventory.Models;

namespace RPInventory.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext (DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; } = default!;
        public DbSet<Department> Departments { get; set; } = default!;
        public DbSet<Profile> Profiles { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<User>().ToTable("User");

            base.OnModelCreating(modelBuilder);
        }
    }
}
