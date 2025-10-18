using Microsoft.EntityFrameworkCore;
using GestionarHuacales.Api.Models;

namespace GestionarHuacales.Api.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<EntradasHuacales> Huacales {  get; set; }
    public DbSet<TipoHuacales> TipoHuacales { get; set; }
    public DbSet<DetalleHuacales> DetalleHuacales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoHuacales>().HasData(
            new TipoHuacales { TipoId = 1, Descripcion = "Pequeña Verde", Existencia = 0 },
            new TipoHuacales { TipoId = 2, Descripcion = "Pequeña Roja", Existencia = 0 },
            new TipoHuacales { TipoId = 3, Descripcion = "Mediana Verde", Existencia = 0 },
            new TipoHuacales { TipoId = 4, Descripcion = "Mediana Roja", Existencia = 0 },
            new TipoHuacales { TipoId = 5, Descripcion = "Jumbo Verde", Existencia = 0 },
            new TipoHuacales { TipoId = 6, Descripcion = "Jumbo Roja", Existencia = 0 }
        );
        base.OnModelCreating(modelBuilder);
    }
}
