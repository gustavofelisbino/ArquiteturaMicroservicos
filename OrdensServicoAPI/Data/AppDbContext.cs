using Microsoft.EntityFrameworkCore;
using OrdensServicoAPI.Models;

namespace OrdensServicoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OrdemServico> OrdensServico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrdemServico>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ClienteId).IsRequired();
                entity.Property(e => e.VeiculoId).IsRequired();
                entity.Property(e => e.DescricaoProblema).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ValorTotal).HasColumnType("decimal(10,2)");
            });
        }
    }
}