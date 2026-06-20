// DTOs/Request/UpdateApplicationRequest.cs
namespace JobMatchBackend.DTOs.Request;

public class UpdateApplicationRequest
{
    public string Status { get; set; } = string.Empty; // "accepted" o "rejected"
}