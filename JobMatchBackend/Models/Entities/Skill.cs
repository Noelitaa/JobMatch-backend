using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.Models.Entities
{
    public class Skill
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        //Many-to-Many relationship with Students (Users)
        public ICollection<User> Students { get; set; } = new List<User>();
    }
}