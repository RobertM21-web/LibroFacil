namespace LibroFacil.Application.DTOs;

/// <summary>
/// DTO de respuesta. Se usa para devolver los datos de un Libro al cliente
/// sin exponer directamente la entidad de dominio.
/// </summary>
public class LibroDto
{
    public int    Id             { get; set; }
    public string ISBN           { get; set; } = string.Empty;
    public string Titulo         { get; set; } = string.Empty;
    public string Autor          { get; set; } = string.Empty;
    public int    AnoPublicacion { get; set; }
    public int    Stock          { get; set; }
}
