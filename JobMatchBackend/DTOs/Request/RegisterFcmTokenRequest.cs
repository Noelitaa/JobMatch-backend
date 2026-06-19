using System.ComponentModel.DataAnnotations;

namespace JobMatchBackend.DTOs.Request;

public class RegisterFcmTokenRequest
{
    [Required(ErrorMessage = "El token FCM es requerido.")]
    public string Token { get; set; } = string.Empty;

    public string? DeviceInfo { get; set; }
}
