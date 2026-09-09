using LibroFacil.Domain.Exceptions;

namespace LibroFacil.Domain.Entities;

/// <summary>
/// Entidad raíz de dominio — Libro.
/// Principio DDD: la entidad se autoprotege. No puede existir un objeto Libro
/// en estado inválido; todas las reglas de negocio se validan en el constructor
/// y en el método Actualizar().
/// </summary>
public class Libro
{
    // ──────────────────────────────────────────────
    // Propiedades (solo lectura desde fuera)
    // ──────────────────────────────────────────────
    public int    Id             { get; private set; }
    public string ISBN           { get; private set; } = string.Empty;
    public string Titulo         { get; private set; } = string.Empty;
    public string Autor          { get; private set; } = string.Empty;
    public int    AnoPublicacion { get; private set; }
    public int    Stock          { get; private set; }

    // ──────────────────────────────────────────────
    // Constructor privado requerido por EF Core
    // (EF lo usa internamente para materializar entidades desde la BD)
    // ──────────────────────────────────────────────
    private Libro() { }

    // ──────────────────────────────────────────────
    // Constructor público — crea un Libro válido
    // ──────────────────────────────────────────────
    /// <summary>
    /// Crea un nuevo libro aplicando todas las reglas de negocio.
    /// Lanza <see cref="DomainException"/> si alguna regla es violada.
    /// </summary>
    public Libro(string isbn, string titulo, string autor, int anoPublicacion, int stock)
    {
        ValidarISBN(isbn);
        ValidarTitulo(titulo);
        ValidarAutor(autor);
        ValidarAnoPublicacion(anoPublicacion);
        ValidarStock(stock);

        ISBN           = isbn.Trim();
        Titulo         = titulo.Trim();
        Autor          = autor.Trim();
        AnoPublicacion = anoPublicacion;
        Stock          = stock;
    }

    // ──────────────────────────────────────────────
    // Método de actualización — reutiliza las mismas validaciones
    // ──────────────────────────────────────────────
    /// <summary>
    /// Actualiza los datos del libro. Aplica las mismas reglas de negocio
    /// que el constructor; nunca deja la entidad en estado inválido.
    /// </summary>
    public void Actualizar(string isbn, string titulo, string autor, int anoPublicacion, int stock)
    {
        ValidarISBN(isbn);
        ValidarTitulo(titulo);
        ValidarAutor(autor);
        ValidarAnoPublicacion(anoPublicacion);
        ValidarStock(stock);

        ISBN           = isbn.Trim();
        Titulo         = titulo.Trim();
        Autor          = autor.Trim();
        AnoPublicacion = anoPublicacion;
        Stock          = stock;
    }

    // ──────────────────────────────────────────────
    // Métodos de validación privados (SRP interno)
    // Cada regla de negocio vive en su propio método
    // ──────────────────────────────────────────────

    // Regla 1: ISBN obligatorio
    private static void ValidarISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new DomainException("El ISBN es obligatorio y no puede estar vacío.");
    }

    // Regla 2: Título obligatorio
    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("El título es obligatorio y no puede estar vacío.");
    }

    // Regla 3: Autor obligatorio
    private static void ValidarAutor(string autor)
    {
        if (string.IsNullOrWhiteSpace(autor))
            throw new DomainException("El autor es obligatorio y no puede estar vacío.");
    }

    // Regla 4: AñoPublicacion > 0 y <= año actual
    private static void ValidarAnoPublicacion(int ano)
    {
        int anoActual = DateTime.UtcNow.Year;
        if (ano <= 0 || ano > anoActual)
            throw new DomainException(
                $"El año de publicación debe ser mayor que 0 y menor o igual al año actual ({anoActual}).");
    }

    // Regla 5: Stock >= 0
    private static void ValidarStock(int stock)
    {
        if (stock < 0)
            throw new DomainException("El stock no puede ser negativo.");
    }
}
