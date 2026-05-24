namespace JobMatchBackend.Models.Entities;

public class FcmToken
{
    public int IdToken { get; set; }
    public Guid IdUser { get; set; }
    public string Token { get; set; } = string.Empty;
    public string? DeviceInfo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? User { get; set; }
}
