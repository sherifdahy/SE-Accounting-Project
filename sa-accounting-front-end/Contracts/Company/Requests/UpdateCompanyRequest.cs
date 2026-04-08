using SA.Accounting.WPF.Contracts.Account.Requests;
using SA.Accounting.WPF.Contracts.Company.Validators;
using SA.Accounting.WPF.Contracts.Owner.Requests;
using SA.Accounting.WPF.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Contracts.Company.Requests;

public class UpdateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<UpdateOwnerRequest> Owners { get; set; } = [];
    public List<UpdateAccountRequest> Accounts { get; set; } = [];
}
