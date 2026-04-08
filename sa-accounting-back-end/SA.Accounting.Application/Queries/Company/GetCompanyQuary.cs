using SA.Accounting.Application.Contracts.Company.Responses;

namespace SA.Accounting.Application.Queries.Company;

public record GetCompanyQuary : IRequest<Result<CompanyDetailResponse>>
{
    public int Id { get; set; }
};

