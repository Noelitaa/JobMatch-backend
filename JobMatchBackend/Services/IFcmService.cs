namespace JobMatchBackend.Services;

public interface IFcmService
{
    Task SendToUserAsync(Guid userId, string title, string body, Dictionary<string, string>? data = null);
    Task SendToTokenAsync(string token, string title, string body, Dictionary<string, string>? data = null);
}
