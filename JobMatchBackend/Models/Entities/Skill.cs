namespace JobMatchBackend.Models.Entities;

public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<User> Students { get; set; } = new List<User>();
}