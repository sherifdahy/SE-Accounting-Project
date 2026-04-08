
namespace SA.Accounting.Application.Commands.Owner;

public record UpdateOwnerCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
};
