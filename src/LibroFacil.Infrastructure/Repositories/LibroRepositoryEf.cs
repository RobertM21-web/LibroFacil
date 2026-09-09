using LibroFacil.Application.Interfaces;
using LibroFacil.Domain.Entities;
using LibroFacil.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibroFacil.Infrastructure.Repositories;

/// <summary>
/// Implementación concreta de ILibroRepository usando Entity Framework Core.
///
/// Principios aplicados:
/// - OCP: si cambiamos de SQL Server a otra base de datos, solo creamos
///        una nueva clase que implemente ILibroRepository — LibroService no se toca.
/// - LSP: cumple exactamente el contrato definido por ILibroRepository.
/// - SRP: esta clase solo tiene responsabilidad de acceso a datos con EF Core.
/// </summary>
public class LibroRepositoryEf : ILibroRepository
{
    private readonly LibroFacilDbContext _context;

    public LibroRepositoryEf(LibroFacilDbContext context)
    {
        _context = context;
    }

    // ──────────────────────────────────────────────
    // Obtener todos
    // ──────────────────────────────────────────────
    public async Task<IEnumerable<Libro>> ObtenerTodosAsync()
    {
        return await _context.Libros
                             .AsNoTracking()
                             .ToListAsync();
    }

    // ──────────────────────────────────────────────
    // Obtener por Id
    // ──────────────────────────────────────────────
    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        return await _context.Libros.FindAsync(id);
    }

    // ──────────────────────────────────────────────
    // Verificar si existe ISBN (con opción de excluir un Id para el PUT)
    // ──────────────────────────────────────────────
    public async Task<bool> ExisteISBNAsync(string isbn, int? excluirId = null)
    {
        return await _context.Libros
                             .AsNoTracking()
                             .AnyAsync(l => l.ISBN == isbn
                                         && (excluirId == null || l.Id != excluirId));
    }

    // ──────────────────────────────────────────────
    // Agregar
    // ──────────────────────────────────────────────
    public async Task AgregarAsync(Libro libro)
    {
        await _context.Libros.AddAsync(libro);
        await _context.SaveChangesAsync();
    }

    // ──────────────────────────────────────────────
    // Actualizar
    // ──────────────────────────────────────────────
    public async Task ActualizarAsync(Libro libro)
    {
        _context.Libros.Update(libro);
        await _context.SaveChangesAsync();
    }

    // ──────────────────────────────────────────────
    // Eliminar
    // ──────────────────────────────────────────────
    public async Task EliminarAsync(int id)
    {
        var libro = await _context.Libros.FindAsync(id);
        if (libro is not null)
        {
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
        }
    }
}
