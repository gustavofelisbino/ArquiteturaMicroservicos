
using Microsoft.EntityFrameworkCore;
using VeiculosAPI.Models;

namespace VeiculosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Veiculo> Veiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Veiculo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClienteId).IsRequired();
                entity.Property(e => e.Marca).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Ano).IsRequired();
                entity.Property(e => e.Placa).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.Placa).IsUnique();
                entity.Property(e => e.Status).HasMaxLength(20);
            });
        }
    }
}