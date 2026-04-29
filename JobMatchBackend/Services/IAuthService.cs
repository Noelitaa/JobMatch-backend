using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IAuthService
{
    LoginResponse Login(LoginRequest request);
}