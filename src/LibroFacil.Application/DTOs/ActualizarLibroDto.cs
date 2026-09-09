namespace LibroFacil.Application.DTOs;

/// <summary>
/// DTO de entrada para actualizar un libro existente.
/// El Id se recibe por la ruta de la URL (no en el body),
/// por eso no se incluye aquí.
/// </summary>
public class ActualizarLibroDto
{
    public string ISBN           { get; set; } = string.Empty;
    public string Titulo         { get; set; } = string.Empty;
    public string Autor          { get; set; } = string.Empty;
    public int    AnoPublicacion { get; set; }
    public int    Stock          { get; set; }
}
