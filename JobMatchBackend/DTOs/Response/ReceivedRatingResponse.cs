namespace JobMatchBackend.DTOs.Response;

public class ReceivedRatingResponse
{
    public int IdRating { get; set; }
    public int IdContract { get; set; }
    public string RaterName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
