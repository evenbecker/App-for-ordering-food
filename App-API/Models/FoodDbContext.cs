using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App_API.Models
{
    public partial class FoodDbContext : DbContext
    {
        public FoodDbContext()
        {
        }

        public FoodDbContext(DbContextOptions<FoodDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<FoodDetail> FoodDetail { get; set; }
        public virtual DbSet<FoodOrder> FoodOrder { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;initial catalog=FoodDb;trusted_connection=yes;TrustServerCertificate=true");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.FoodName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoginId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.FoodCodeNavigation)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.FoodCode)
                    .HasConstraintName("FK__Cart__FoodCode__3B75D760");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.LoginId)
                    .HasConstraintName("FK__Cart__LoginId__3A81B327");
            });

            modelBuilder.Entity<FoodDetail>(entity =>
            {
                entity.HasKey(e => e.FoodCode)
                    .HasName("PK__FoodDeta__58AA23F9C70276D4");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FoodName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FoodOrder>(entity =>
            {
                entity.HasKey(e => e.OrderNo)
                    .HasName("PK__FoodOrde__C3907C74C6FB7786");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FoodName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoginId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.FoodCodeNavigation)
                    .WithMany(p => p.FoodOrder)
                    .HasForeignKey(d => d.FoodCode)
                    .HasConstraintName("FK__FoodOrder__FoodC__3F466844");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.FoodOrder)
                    .HasForeignKey(d => d.LoginId)
                    .HasConstraintName("FK__FoodOrder__Login__3E52440B");
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PK__UserDeta__4DDA28180FB38B17");

                entity.Property(e => e.LoginId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
