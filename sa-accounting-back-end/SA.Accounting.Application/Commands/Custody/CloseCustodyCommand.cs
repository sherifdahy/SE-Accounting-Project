using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Custody;

public record CloseCustodyCommand(int Id) : IRequest<Result>;
