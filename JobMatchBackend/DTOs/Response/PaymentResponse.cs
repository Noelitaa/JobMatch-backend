namespace JobMatchBackend.DTOs.Response;

public class PaymentResponse
{
    public int IdPayment { get; set; }
    public int IdContract { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? ReceiptUrl { get; set; }
    public string? Concept { get; set; }
}
