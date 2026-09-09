using LibroFacil.Application.DTOs;
using LibroFacil.Application.Interfaces;
using LibroFacil.Domain.Entities;
using LibroFacil.Domain.Exceptions;

namespace LibroFacil.Application.Services;

/// <summary>
/// Orquesta los casos de uso relacionados a Libros.
///
/// Principios aplicados:
/// - SRP: solo contiene lógica de aplicación/orquestación, no HTTP ni EF Core.
/// - DIP: recibe ILibroRepository por constructor (inyección de dependencias).
/// - La unicidad del ISBN se valida aquí porque es una regla que requiere
///   consultar la base de datos (no puede vivir solo en la entidad).
/// </summary>
public class LibroService
{
    private readonly ILibroRepository _repository;

    // DIP en acción: el servicio solo conoce la interfaz, nunca la implementación concreta
    public LibroService(ILibroRepository repository)
    {
        _repository = repository;
    }

    // ──────────────────────────────────────────────
    // Caso de uso: Listar todos los libros
    // ──────────────────────────────────────────────
    public async Task<IEnumerable<LibroDto>> ObtenerTodosAsync()
    {
        var libros = await _repository.ObtenerTodosAsync();
        return libros.Select(MapearADto);
    }

    // ──────────────────────────────────────────────
    // Caso de uso: Consultar libro por Id
    // ──────────────────────────────────────────────
    public async Task<LibroDto?> ObtenerPorIdAsync(int id)
    {
        var libro = await _repository.ObtenerPorIdAsync(id);
        return libro is null ? null : MapearADto(libro);
    }

    // ──────────────────────────────────────────────
    // Caso de uso: Agregar un nuevo libro
    // ──────────────────────────────────────────────
    public async Task<LibroDto> AgregarAsync(CrearLibroDto dto)
    {
        // Regla de negocio: ISBN único (cross-entidad → vive en el servicio)
        bool isbnExiste = await _repository.ExisteISBNAsync(dto.ISBN);
        if (isbnExiste)
            throw new DomainException($"Ya existe un libro con el ISBN '{dto.ISBN}'.");

        // El constructor de Libro valida las demás reglas internamente (DDD)
        var libro = new Libro(dto.ISBN, dto.Titulo, dto.Autor, dto.AnoPublicacion, dto.Stock);

        await _repository.AgregarAsync(libro);
        return MapearADto(libro);
    }

    // ──────────────────────────────────────────────
    // Caso de uso: Actualizar un libro existente
    // ──────────────────────────────────────────────
    public async Task ActualizarAsync(int id, ActualizarLibroDto dto)
    {
        var libro = await _repository.ObtenerPorIdAsync(id);
        if (libro is null)
            throw new DomainException($"No se encontró un libro con Id {id}.");

        // Regla de negocio: ISBN único, excluyendo el propio libro que se edita
        bool isbnExiste = await _repository.ExisteISBNAsync(dto.ISBN, excluirId: id);
        if (isbnExiste)
            throw new DomainException($"Ya existe otro libro con el ISBN '{dto.ISBN}'.");

        // Libro.Actualizar() aplica las mismas validaciones internas (DDD)
        libro.Actualizar(dto.ISBN, dto.Titulo, dto.Autor, dto.AnoPublicacion, dto.Stock);

        await _repository.ActualizarAsync(libro);
    }

    // ──────────────────────────────────────────────
    // Caso de uso: Eliminar un libro
    // ──────────────────────────────────────────────
    public async Task EliminarAsync(int id)
    {
        var libro = await _repository.ObtenerPorIdAsync(id);
        if (libro is null)
            throw new DomainException($"No se encontró un libro con Id {id}.");

        await _repository.EliminarAsync(id);
    }

    // ──────────────────────────────────────────────
    // Mapeo privado: Entidad de dominio → DTO de respuesta
    // (evita exponer la entidad directamente a la capa API)
    // ──────────────────────────────────────────────
    private static LibroDto MapearADto(Libro libro) => new()
    {
        Id             = libro.Id,
        ISBN           = libro.ISBN,
        Titulo         = libro.Titulo,
        Autor          = libro.Autor,
        AnoPublicacion = libro.AnoPublicacion,
        Stock          = libro.Stock
    };
}
