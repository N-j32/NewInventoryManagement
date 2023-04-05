using InventoryManagement.Models;
using InventoryManagement.Models.Account;
using InventoryManagement.Models.Cascade;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Data
{
    public  class ApplicationContext : DbContext
    {
        

        public ApplicationContext( DbContextOptions <ApplicationContext> options) : base(options)
        {
        }
        public virtual DbSet<Unit> Units { get; set; }
        public DbSet<Category> Categories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<ProductProfile> ProductProfiles { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<OrderDetail>OrderDetails { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        

    }
}
