using LibroFacil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibroFacil.Infrastructure.Persistence;

/// <summary>
/// Contexto de base de datos EF Core para LibroFácil.
/// Mapea la entidad de dominio Libro a la tabla "Libros" en SQL Server.
/// </summary>
public class LibroFacilDbContext : DbContext
{
    public LibroFacilDbContext(DbContextOptions<LibroFacilDbContext> options)
        : base(options) { }

    public DbSet<Libro> Libros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.ToTable("Libros");

            entity.HasKey(l => l.Id);

            entity.Property(l => l.ISBN)
                  .IsRequired()
                  .HasMaxLength(20);

            // Índice único sobre ISBN — garantiza unicidad a nivel de base de datos
            entity.HasIndex(l => l.ISBN)
                  .IsUnique();

            entity.Property(l => l.Titulo)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(l => l.Autor)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(l => l.AnoPublicacion)
                  .IsRequired();

            entity.Property(l => l.Stock)
                  .IsRequired();
        });
    }
}
