using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LipaNaMpesaAPI.Models
{
    public partial class MpesaSettingsContext : DbContext
    {
        public MpesaSettingsContext()
        {
        }

        public MpesaSettingsContext(DbContextOptions<MpesaSettingsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Setting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DatabaseConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.BusinessCode).IsRequired();

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ConsumerKey).IsRequired();

                entity.Property(e => e.ConsumerSecret).IsRequired();

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.PassKey).IsRequired();

                entity.Property(e => e.TransactionDesc).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
