using JobMatchBackend.DTOs.Request;
using JobMatchBackend.DTOs.Response;
using JobMatchBackend.Models.Entities;
using JobMatchBackend.Repositories;

namespace JobMatchBackend.Services;

public class PaymentService : IPaymentService
{
    private static readonly string[] ValidPaymentMethods = { "transfer", "cash", "sinpe" };

    public  readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<List<PaymentResponse>> GetPaymentHistoryAsync(Guid userId, DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
            throw new InvalidOperationException("La fecha de inicio debe ser anterior o igual a la fecha de fin.");

        var payments = await _paymentRepository.GetPaymentHistoryByUserIdAsync(userId, startDate, endDate);
        return payments.Select(payment => ToResponse(payment, userId)).ToList();
    }

    public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, Guid callerId)
    {
        if (request.Amount <= 0)
            throw new InvalidOperationException("El monto debe ser mayor a 0.");

        if (string.IsNullOrWhiteSpace(request.PaymentMethod) ||
            !ValidPaymentMethods.Contains(request.PaymentMethod, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException("El método de pago debe ser 'transfer', 'cash' o 'sinpe'.");

        var contract = await _paymentRepository.GetContractByIdAsync(request.IdContract);
        if (contract == null)
            throw new KeyNotFoundException("Contrato no encontrado.");

        if (!string.Equals(contract.Status, "active", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("El pago debe estar vinculado a un contrato activo.");

        if (contract.IdCompany != callerId)
            throw new UnauthorizedAccessException("No tienes permiso para registrar pagos en este contrato.");

        var payment = new Payment
        {
            IdContract = request.IdContract,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod!,
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ReceiptUrl = request.Receipt
        };

        var createdPayment = await _paymentRepository.CreatePaymentAsync(payment);
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