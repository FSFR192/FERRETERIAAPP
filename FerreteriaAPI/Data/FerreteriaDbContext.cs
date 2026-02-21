using Microsoft.EntityFrameworkCore;
using FerreteriaAPI.Models;
using System.Security.Cryptography.X509Certificates;

namespace FerreteriaAPI.Data
{
    public class FerreteriaDbContext : DbContext
    {
        public FerreteriaDbContext(DbContextOptions<FerreteriaDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<DetalleVenta>()
        .HasOne(d => d.Producto)
        .WithMany()
        .HasForeignKey(d => d.ProductoId)
        .OnDelete(DeleteBehavior.SetNull); // Evita que se borre un producto si tiene detalles de venta asociados
        }

    }
}