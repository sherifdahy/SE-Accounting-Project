using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Platform.Responses;

public record PlatformResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
