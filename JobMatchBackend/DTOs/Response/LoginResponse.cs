namespace JobMatchBackend.DTOs.Response;

public class LoginResponse
{
    public string? Email {get; set;}
    public Guid UserId {get; set;} 
    public string? Role {get; set;} 
    public string? FullName {get; set;}
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
}