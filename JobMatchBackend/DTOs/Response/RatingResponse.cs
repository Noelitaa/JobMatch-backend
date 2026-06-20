namespace JobMatchBackend.DTOs.Response;

public class RatingResponse
{
    public int IdRating { get; set; }
    public int IdContract { get; set; }
    public Guid IdRated { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
