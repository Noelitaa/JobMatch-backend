using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class CreateRatingRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "El ID del contrato no es válido.")]
    public int IdContract { get; set; }

    [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5 estrellas.")]
    public int Stars { get; set; }

    public string? Comment { get; set; }
}
