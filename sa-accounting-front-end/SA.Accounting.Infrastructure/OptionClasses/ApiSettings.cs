using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Infrastructure.OptionClasses;

public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public bool IgnoreSslErrors { get; set; } = false;
}


