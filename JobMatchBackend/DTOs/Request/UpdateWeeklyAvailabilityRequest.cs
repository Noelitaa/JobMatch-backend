namespace JobMatchBackend.DTOs.Request;

public class UpdateWeeklyAvailabilityRequest
{
    public List<WeeklyAvailabilityBlockRequest> TimeBlocks { get; set; } = new();
}

public class WeeklyAvailabilityBlockRequest
{
    public int Day { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
