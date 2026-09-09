namespace LibroFacil.Domain.Exceptions;

/// <summary>
/// Excepción base para todas las violaciones a las reglas de negocio del dominio.
/// Cualquier regla rota dentro de la entidad Libro lanzará esta excepción.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
