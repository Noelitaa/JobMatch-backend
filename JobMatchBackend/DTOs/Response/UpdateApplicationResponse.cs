// DTOs/Response/UpdateApplicationResponse.cs
namespace JobMatchBackend.DTOs.Response;

public class UpdateApplicationResponse
{
    public int IdApplication { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}