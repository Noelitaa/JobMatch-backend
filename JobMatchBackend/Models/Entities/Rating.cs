namespace JobMatchBackend.Models.Entities;

public class Rating
{
    public int IdRating { get; set; }
    public int IdContract { get; set; }
    public Guid IdRater { get; set; }
    public Guid IdRated { get; set; }
    public int Stars { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Contract? Contract { get; set; }
    public virtual User? Rater { get; set; }
    public virtual User? Rated { get; set; }
}
