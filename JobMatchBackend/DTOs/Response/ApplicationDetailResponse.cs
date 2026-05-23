namespace JobMatchBackend.DTOs.Response;

public class ApplicationDetailResponse
{
    public int ApplicationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int JobId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime CreatedAt { get; set; }
}
