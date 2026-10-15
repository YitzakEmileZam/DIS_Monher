using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DIS_Monher.Models;

namespace DIS_Monher.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Material> Materiales { get; set; }
    public DbSet<MovimientoStock> MovimientosStock { get; set; }
    public DbSet<OrdenProduccion> OrdenesProduccion { get; set; }
    public DbSet<AvanceProduccion> AvancesProduccion { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    base.OnModelCreating(builder);

    builder.Entity<Material>()
        .HasIndex(m => m.Codigo)
        .IsUnique();

    builder.Entity<AvanceProduccion>()
    .Property(a => a.Id)
    .ValueGeneratedOnAdd();
    }
}
