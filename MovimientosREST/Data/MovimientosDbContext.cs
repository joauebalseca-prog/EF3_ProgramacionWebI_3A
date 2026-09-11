using Microsoft.EntityFrameworkCore;
using MovimientosREST.Models;

namespace MovimientosREST.Data;

public class MovimientosDbContext : DbContext
{
    public MovimientosDbContext(DbContextOptions<MovimientosDbContext> options) : base(options) { }

    public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovimientoInventario>()
            .ToTable("MovimientosInventario")
            .HasKey(x => x.IdMovimiento);
    }
}
