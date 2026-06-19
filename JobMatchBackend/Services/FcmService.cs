using FirebaseAdmin.Messaging;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class FcmService : IFcmService
{
    private readonly IFcmTokenRepository _fcmTokenRepository;
    private readonly ILogger<FcmService> _logger;

    public FcmService(IFcmTokenRepository fcmTokenRepository, ILogger<FcmService> logger)
    {
        _fcmTokenRepository = fcmTokenRepository;
        _logger = logger;
    }

    public async Task SendToUserAsync(Guid userId, string title, string body, Dictionary<string, string>? data = null)
    {
        var tokens = await _fcmTokenRepository.GetTokensByUserIdAsync(userId);
        if (tokens.Count == 0) return;

        var tasks = tokens.Select(token => SendToTokenAsync(token, title, body, data));
        await Task.WhenAll(tasks);
    }

    public async Task SendToTokenAsync(string token, string title, string body, Dictionary<string, string>? data = null)
    {
        try
        {
            var message = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? new Dictionary<string, string>(),
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = title,
                        Body = body,
                        Sound = "default"
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "default",
                        Badge = 1
                    }
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
        catch (FirebaseMessagingException ex)
            when (ex.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                  ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
        {
            // Token is no longer valid — remove it silently
            _logger.LogInformation("Removing invalid FCM token: {Token}", token[..Math.Min(20, token.Length)]);
            await _fcmTokenRepository.DeleteByTokenAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send FCM notification to token.");
        }
    }
}
