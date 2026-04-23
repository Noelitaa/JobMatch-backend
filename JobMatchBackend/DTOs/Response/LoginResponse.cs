namespace JobMatchBackend.DTOs.Response;

public class LoginResponse
{
    public string? email {get; set;}
    public string? UserId {get; set;} 
    public string? Role {get; set;} 
    public string? FullName {get; set;}
    public string? Token { get; set; }
}