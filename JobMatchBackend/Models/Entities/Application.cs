namespace JobMatchBackend.Models.Entities;

public class Application
{
    public int IdApplication { get; set; }
    public int IdJob { get; set; }
    public Guid IdStudent { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual Job? Job { get; set; }
    public virtual User? Student { get; set; }
    public virtual Contract? Contract { get; set; }
}