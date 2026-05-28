namespace JobMatchBackend.DTOs.Response;

public class ContractAcceptResponse
{
    public ContractResponse Contract { get; set; } = new();
    public JobResponse Job { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
