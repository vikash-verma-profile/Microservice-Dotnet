using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inventory.Models
{
    public partial class EshoppingContext : DbContext
    {
        public EshoppingContext()
        {
        }

        public EshoppingContext(DbContextOptions<EshoppingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblLogin> TblLogins { get; set; } = null!;
        public virtual DbSet<TblProduct> TblProducts { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblLogin>(entity =>
            {
                entity.ToTable("tblLogin");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Status).ValueGeneratedOnAdd();

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tblProduct");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
