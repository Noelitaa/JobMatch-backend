using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<List<PaymentResponse>> GetPaymentHistoryAsync(Guid userId, DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            throw new InvalidOperationException("Start date must be before or equal to end date");

        var payments = await _paymentRepository.GetPaymentHistoryByUserIdAsync(userId, startDate, endDate);
        return payments.Select(payment => ToResponse(payment, userId)).ToList();
    }

    private static PaymentResponse ToResponse(Payment payment, Guid userId)
    {
        return new PaymentResponse
        {
            IdPayment = payment.IdPayment,
            IdContract = payment.IdContract,
            Amount = payment.Amount,
            PaymentMethod = payment.PaymentMethod,
            Date = payment.PaymentDate,
            Type = payment.Contract?.IdStudent == userId ? "received" : "made",
            ReceiptUrl = payment.ReceiptUrl,
            Concept = payment.Contract?.Job?.Title
        };
    }
}
