using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CosmeticShop
{
    public partial class DataBase : DbContext
    {
        public DataBase()
            : base("name=DataBase")
        {
        }

        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPhoto> ProductPhoto { get; set; }
        public virtual DbSet<ProductSale> ProductSale { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.Cost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductPhoto)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductSale)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Product1)
                .WithMany(e => e.Product2)
                .Map(m => m.ToTable("AttachedProduct").MapLeftKey("MainProductID").MapRightKey("AttachedProductID"));
        }
    }
}
