using SA.Accounting.Application.Contracts.Owner.Responses;

namespace SA.Accounting.Application.Queries.Owner;

public record GetOwnerQuary : IRequest<Result<OwnerResponse>>
{
    public int Id { get; set; }
};
