namespace SA.Accounting.Core.Contracts.Owner.Requests;

public sealed class CreateOwnerRequest
{
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
}
