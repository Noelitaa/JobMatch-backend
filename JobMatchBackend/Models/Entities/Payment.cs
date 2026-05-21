namespace JobMatchBackend.Models.Entities;

public class Payment
{
    public int IdPayment { get; set; }
    public int IdContract { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateOnly PaymentDate { get; set; }
    public string? ReceiptUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Contract? Contract { get; set; }
}
