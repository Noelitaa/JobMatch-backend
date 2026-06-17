namespace JobMatchBackend.DTOs.Request;

public class CreatePaymentRequest
{
    public string ContractId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Receipt { get; set; }
}