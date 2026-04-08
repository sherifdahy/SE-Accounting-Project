using SA.Accounting.WPF.Contracts.Common;
using SA.Accounting.WPF.Contracts.Company.Validators;
using SA.Accounting.WPF.Contracts.Owner.Validators;
using SA.Accounting.WPF.Core;
using Telerik.Windows.Documents.Spreadsheet.Core;

namespace SA.Accounting.WPF.Contracts.Owner.Requests;

public sealed class CreateOwnerRequest
{
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
}