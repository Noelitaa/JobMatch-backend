namespace JobMatchBackend.Models.Entities;

public class ModerationLog
{
    public int IdLog { get; set; }
    public Guid AdminUserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public int TargetId { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? AdminUser { get; set; }
}
