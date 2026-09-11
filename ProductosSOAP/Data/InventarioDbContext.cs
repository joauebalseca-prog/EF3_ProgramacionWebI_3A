using Microsoft.EntityFrameworkCore;
using ProductosSOAP.Models;

namespace ProductosSOAP.Data;

public class InventarioDbContext : DbContext
{
    public InventarioDbContext(DbContextOptions<InventarioDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().ToTable("Categorias").HasKey(x => x.IdCategoria);
        modelBuilder.Entity<Producto>().ToTable("Productos").HasKey(x => x.IdProducto);

        modelBuilder.Entity<Producto>()
            .Property(x => x.Precio)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Producto>()
            .HasOne<Categoria>()
            .WithMany()
            .HasForeignKey(x => x.IdCategoria);
    }
}
