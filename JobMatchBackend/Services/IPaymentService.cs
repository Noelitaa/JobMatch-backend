using JobMatchBackend.DTOs.Response;

namespace JobMatchBackend.Services;

public interface IPaymentService
{
    Task<List<PaymentResponse>> GetPaymentHistoryAsync(Guid userId, DateOnly? startDate, DateOnly? endDate);
}
