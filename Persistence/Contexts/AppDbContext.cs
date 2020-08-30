using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using Supermarket.API.Domain.Models;

namespace Supermarket.API.Persistence.Contexts
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Item>().ToTable("Items");
            builder.Entity<Item>().HasKey(p => p.Id);
            builder.Entity<Item>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();//.HasValueGenerator<InMemoryIntegerValueGenerator<int>>();
            builder.Entity<Item>().Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Entity<Item>().HasMany(p => p.Products).WithOne(p => p.Item).HasForeignKey(p => p.ItemId);

            builder.Entity<Item>().HasData
            (
                new Item { Id = 100, Name = "Fruits and Vegetables" }, // Id set manually due to in-memory provider
                new Item { Id = 101, Name = "Dairy" }
            );

            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Product>().HasKey(p => p.Id);
            builder.Entity<Product>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Product>().Property(p => p.QuantityInPackage).IsRequired();
            builder.Entity<Product>().Property(p => p.UnitOfMeasurement).IsRequired();

            builder.Entity<Product>().HasData
            (
                new Product
                {
                    Id = 100,
                    Name = "Apple",
                    QuantityInPackage = 1,
                    UnitOfMeasurement = EUnitOfMeasurement.Unity,
                    ItemId = 100
                },
                new Product
                {
                    Id = 101,
                    Name = "Milk",
                    QuantityInPackage = 2,
                    UnitOfMeasurement = EUnitOfMeasurement.Liter,
                    ItemId = 101,
                }
            );
            
            builder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__1788CC4C1F5C1650");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
            
            OnModelCreatingPartial(builder);
        }
        
          partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}