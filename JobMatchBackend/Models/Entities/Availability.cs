namespace JobMatchBackend.Models.Entities;

public class Availability
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    // Navigation
    public virtual User? Student { get; set; }
}
