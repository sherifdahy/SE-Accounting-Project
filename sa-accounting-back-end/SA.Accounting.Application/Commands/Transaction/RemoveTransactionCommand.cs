using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Transaction;

public record RemoveTransactionCommand : IRequest<Result>
{
    public int Id { get; set; }
}
