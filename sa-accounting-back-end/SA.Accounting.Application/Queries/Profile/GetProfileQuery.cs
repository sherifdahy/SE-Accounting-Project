using SA.Accounting.Application.Contracts.Profile.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Profile;

public record GetProfileQuery : IRequest<Result<ProfileResponse>>
{
    
}
