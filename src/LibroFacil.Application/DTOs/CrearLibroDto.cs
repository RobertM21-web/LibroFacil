namespace LibroFacil.Application.DTOs;

/// <summary>
/// DTO de entrada para crear un nuevo libro.
/// Los datos llegan desde el Controller (capa API) y el Service
/// los usa para construir la entidad Libro.
/// </summary>
public class CrearLibroDto
{
    public string ISBN           { get; set; } = string.Empty;
    public string Titulo         { get; set; } = string.Empty;
    public string Autor          { get; set; } = string.Empty;
    public int    AnoPublicacion { get; set; }
    public int    Stock          { get; set; }
}
