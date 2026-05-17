using JobMatchBackend.DTOs.Response.Student;
using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Mappers;

public static class StudentMapper
{
    public static StudentProfileResponse ToResponse(User user)
    {
        return new StudentProfileResponse
        {
            Id = user.Id,
            User = new UserProfileResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Avatar = user.AvatarUrl,
                Phone = user.Phone,
                Bio = user.Bio,
                Active = user.IsActive
            },
            University = user.University,
            Career = user.Career,
            StudentId = user.StudentId,
            Skills = user.StudentSkills?
                .Select(ss => ss.Skill!.Name)
                .ToList() ?? new List<string>(),
            Availability = new AvailabilityResponse
            {
                Sunday    = GetSlotsForDay(user.Availabilities, 0),
                Monday    = GetSlotsForDay(user.Availabilities, 1),
                Tuesday   = GetSlotsForDay(user.Availabilities, 2),
                Wednesday = GetSlotsForDay(user.Availabilities, 3),
                Thursday  = GetSlotsForDay(user.Availabilities, 4),
                Friday    = GetSlotsForDay(user.Availabilities, 5),
                Saturday  = GetSlotsForDay(user.Availabilities, 6)
            },
            AverageRating = 0.0f
        };
    }

    private static List<TimeSlot> GetSlotsForDay(ICollection<Availability>? availabilities, int dayOfWeek)
    {
        return availabilities?
            .Where(a => a.DayOfWeek == dayOfWeek)
            .Select(a => new TimeSlot
            {
                Start = a.StartTime.ToString("HH:mm"),
                End   = a.EndTime.ToString("HH:mm")
            })
            .ToList() ?? new List<TimeSlot>();
    }
}
