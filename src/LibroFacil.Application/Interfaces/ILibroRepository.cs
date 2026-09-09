using LibroFacil.Domain.Entities;

namespace LibroFacil.Application.Interfaces;

/// <summary>
/// Contrato del repositorio de Libros.
/// 
/// Principios aplicados:
/// - ISP: solo métodos relacionados a la entidad Libro.
/// - DIP: LibroService depende de esta interfaz, nunca de EF Core directamente.
/// - OCP: si cambia la base de datos, se crea una nueva implementación de esta
///        interfaz sin tocar el servicio.
/// </summary>
public interface ILibroRepository
{
    /// <summary>Retorna todos los libros.</summary>
    Task<IEnumerable<Libro>> ObtenerTodosAsync();

    /// <summary>Retorna un libro por su Id, o null si no existe.</summary>
    Task<Libro?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Verifica si ya existe un libro con el ISBN indicado.
    /// El parámetro <paramref name="excluirId"/> permite ignorar el libro
    /// que se está actualizando (evitar falso positivo en PUT).
    /// </summary>
    Task<bool> ExisteISBNAsync(string isbn, int? excluirId = null);

    /// <summary>Persiste un nuevo libro en la base de datos.</summary>
    Task AgregarAsync(Libro libro);

    /// <summary>Marca un libro existente como modificado y persiste los cambios.</summary>
    Task ActualizarAsync(Libro libro);

    /// <summary>Elimina el libro con el Id indicado.</summary>
    Task EliminarAsync(int id);
}
