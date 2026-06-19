using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class CreatePaymentRequest
{
    [Required(ErrorMessage = "El ID del contrato es requerido.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del contrato no es válido.")]
    public int IdContract { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Receipt { get; set; }
}