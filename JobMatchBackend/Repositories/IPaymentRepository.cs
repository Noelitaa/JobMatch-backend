using JobMatchBackend.Models.Entities;

namespace JobMatchBackend.Repositories;

public interface IPaymentRepository
{
    Task<List<Payment>> GetPaymentHistoryByUserIdAsync(Guid userId, DateOnly? startDate, DateOnly? endDate);
}
