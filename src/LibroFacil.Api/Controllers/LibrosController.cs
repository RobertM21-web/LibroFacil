using LibroFacil.Application.DTOs;
using LibroFacil.Application.Services;
using LibroFacil.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LibroFacil.Api.Controllers;

/// <summary>
/// Controlador REST para el recurso Libros.
///
/// Principios aplicados:
/// - SRP: el controlador SOLO se encarga de recibir peticiones HTTP,
///        delegar al servicio y devolver respuestas HTTP.
///        No contiene lógica de negocio.
/// - DIP: recibe LibroService por constructor (inyección de dependencias).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly LibroService _service;

    public LibrosController(LibroService service)
    {
        _service = service;
    }

    // ──────────────────────────────────────────────
    // GET /api/libros
    // Retorna la lista completa de libros.
    // ──────────────────────────────────────────────
    /// <summary>Lista todos los libros registrados.</summary>
    /// <response code="200">Lista de libros (puede estar vacía).</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LibroDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var libros = await _service.ObtenerTodosAsync();
        return Ok(libros);
    }

    // ──────────────────────────────────────────────
    // GET /api/libros/{id}
    // Retorna un libro por su Id.
    // ──────────────────────────────────────────────
    /// <summary>Consulta un libro por su Id.</summary>
    /// <response code="200">El libro encontrado.</response>
    /// <response code="404">No existe un libro con ese Id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LibroDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var libro = await _service.ObtenerPorIdAsync(id);

        if (libro is null)
            return NotFound(new { mensaje = $"No se encontró un libro con Id {id}." });

        return Ok(libro);
    }

    // ──────────────────────────────────────────────
    // POST /api/libros
    // Crea un nuevo libro.
    // ──────────────────────────────────────────────
    /// <summary>Agrega un nuevo libro.</summary>
    /// <response code="201">Libro creado exitosamente.</response>
    /// <response code="400">Datos inválidos o violación de regla de negocio.</response>
    [HttpPost]
    [ProducesResponseType(typeof(LibroDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Agregar([FromBody] CrearLibroDto dto)
    {
        try
        {
            var libro = await _service.AgregarAsync(dto);

            // 201 Created + cabecera Location apuntando al recurso recién creado
            return CreatedAtAction(nameof(ObtenerPorId), new { id = libro.Id }, libro);
        }
        catch (DomainException ex)
        {
            // Regla de negocio violada → 400 Bad Request con mensaje descriptivo
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // ──────────────────────────────────────────────
    // PUT /api/libros/{id}
    // Actualiza un libro existente.
    // ──────────────────────────────────────────────
    /// <summary>Actualiza los datos de un libro existente.</summary>
    /// <response code="204">Libro actualizado correctamente (sin contenido).</response>
    /// <response code="400">Datos inválidos o violación de regla de negocio.</response>
    /// <response code="404">No existe un libro con ese Id.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarLibroDto dto)
    {
        try
        {
            await _service.ActualizarAsync(id, dto);
            return NoContent(); // 204: éxito sin cuerpo de respuesta
        }
        catch (DomainException ex)
        {
            // Distinguimos 404 de 400 por el contenido del mensaje
            if (ex.Message.Contains("No se encontró"))
                return NotFound(new { mensaje = ex.Message });

            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // ──────────────────────────────────────────────
    // DELETE /api/libros/{id}
    // Elimina un libro por su Id.
    // ──────────────────────────────────────────────
    /// <summary>Elimina un libro por su Id.</summary>
    /// <response code="204">Libro eliminado correctamente (sin contenido).</response>
    /// <response code="404">No existe un libro con ese Id.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _service.EliminarAsync(id);
            return NoContent(); // 204: éxito sin cuerpo de respuesta
        }
        catch (DomainException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }
}
