namespace JobMatchBackend.Models.Entities;

public class User
{
    public Guid Id { get; set; }  
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; } = false;
    
    // STUDENT
    public string? University { get; set; }
    public string? Career { get; set; }
    public string? StudentId { get; set; }
    
    // COMPANY
    public string? CompanyName { get; set; }
    public string? Description { get; set; }
    public string? CompanyId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Contract>? CompanyContracts { get; set; }
    public virtual ICollection<Contract>? StudentContracts { get; set; }
    public virtual ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
}