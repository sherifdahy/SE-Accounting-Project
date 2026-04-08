namespace SA.Accounting.WPF.Contracts.Company.Responses;

public record CompanyResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
