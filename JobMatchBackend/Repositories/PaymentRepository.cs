using JobMatchBackend.Data;
using JobMatchBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMatchBackend.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _dbContext;

    public PaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Payment>> GetPaymentHistoryByUserIdAsync(Guid userId, DateOnly? startDate, DateOnly? endDate)
    {
        var query = _dbContext.Payments
            .Include(payment => payment.Contract)
                .ThenInclude(contract => contract!.Job)
            .Where(payment =>
                payment.Contract != null &&
                (payment.Contract.IdStudent == userId || payment.Contract.IdCompany == userId));

        if (startDate.HasValue)
            query = query.Where(payment => payment.PaymentDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(payment => payment.PaymentDate <= endDate.Value);

        return await query
            .OrderByDescending(payment => payment.PaymentDate)
            .ThenByDescending(payment => payment.CreatedAt)
            .ToListAsync();
    }

    public async Task<Contract?> GetContractByIdAsync(int contractId)
    {
        return await _dbContext.Contracts
            .Include(contract => contract.Job)
            .FirstOrDefaultAsync(contract => contract.IdContract == contractId);
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();
        return payment;
    }
}