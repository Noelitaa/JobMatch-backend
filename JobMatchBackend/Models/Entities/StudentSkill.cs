namespace JobMatchBackend.Models.Entities;

public class StudentSkill
{
    public Guid StudentId { get; set; }
    public Guid SkillId { get; set; }

    // Navigation
    public virtual User? Student { get; set; }
    public virtual Skill? Skill { get; set; }
}
