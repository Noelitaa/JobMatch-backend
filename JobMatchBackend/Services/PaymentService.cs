using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class PaymentService : IPaymentService
{
    private static readonly string[] ValidPaymentMethods = { "transfer", "cash", "sinpe" };

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

    public async Task<PaymentResponse> RegisterPaymentAsync(CreatePaymentRequest request, Guid callerId)
    {
        if (!int.TryParse(request.ContractId, out var contractId))
            throw new InvalidOperationException("Invalid contract id");

        if (request.Amount <= 0)
            throw new InvalidOperationException("Amount must be greater than zero");

        if (!string.IsNullOrWhiteSpace(request.PaymentMethod) &&
            !ValidPaymentMethods.Contains(request.PaymentMethod, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException("Payment method must be one of: transfer, cash, sinpe");

        var contract = await _paymentRepository.GetContractByIdAsync(contractId);
        if (contract == null)
            throw new InvalidOperationException("Contract not found");

        if (!string.Equals(contract.Status, "active", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Payment must be linked to an active contract");

        var payment = new Payment
        {
            IdContract = contractId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod ?? string.Empty,
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ReceiptUrl = request.Receipt
        };

        var createdPayment = await _paymentRepository.AddPaymentAsync(payment);
        createdPayment.Contract = contract;

        return ToResponse(createdPayment, callerId);
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