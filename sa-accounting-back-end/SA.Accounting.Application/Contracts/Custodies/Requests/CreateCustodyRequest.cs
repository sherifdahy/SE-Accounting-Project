using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Custodies.Requests;

public class CreateCustodyRequest
{
    public int UserId { get; set; }
    public string? Note { get; set; }
}
