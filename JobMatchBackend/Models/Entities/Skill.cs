namespace JobMatchBackend.Models.Entities;

public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public virtual ICollection<StudentSkill>? StudentSkills { get; set; }
}
