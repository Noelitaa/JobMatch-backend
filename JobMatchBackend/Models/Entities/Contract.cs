// Models/Entities/Contract.cs
namespace JobMatchBackend.Models.Entities;

public class Contract
{
    public int IdContract { get; set; }
    public int IdApplication { get; set; }
    public int IdJob { get; set; }
    public Guid IdStudent { get; set; }
    public Guid IdCompany { get; set; }
    public string? Status { get; set; }  
    public string? ContractData { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }

    // Navigation properties
    public virtual Application? Application { get; set; }
    public virtual Job? Job { get; set; }
    public virtual User? Student { get; set; }
    public virtual User? Company { get; set; }
}