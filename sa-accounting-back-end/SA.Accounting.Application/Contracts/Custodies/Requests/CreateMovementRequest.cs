using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Custodies.Requests;

public class CreateMovementRequest
{
    public MovementType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}
