using Microsoft.EntityFrameworkCore;
using ClientesAPI.Models;

namespace ClientesAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CPF).IsRequired().HasMaxLength(11);
                entity.HasIndex(e => e.CPF).IsUnique();
                entity.Property(e => e.Telefone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.Property(e => e.Endereco).HasMaxLength(500);
            });
        }
    }
}