namespace JobMatchBackend.DTOs.Response.Student;

public class AvailabilityResponse
{
    public List<TimeSlot> Sunday { get; set; } = new();
    public List<TimeSlot> Monday { get; set; } = new();
    public List<TimeSlot> Tuesday { get; set; } = new();
    public List<TimeSlot> Wednesday { get; set; } = new();
    public List<TimeSlot> Thursday { get; set; } = new();
    public List<TimeSlot> Friday { get; set; } = new();
    public List<TimeSlot> Saturday { get; set; } = new();
}
