using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.Contracts.Company.Validators;
using SA.Accounting.Core.Contracts.Owner.Requests;
using SA.Accounting.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Contracts.Company.Requests;

public class UpdateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<UpdateOwnerRequest> Owners { get; set; } = [];
    public List<UpdateAccountRequest> Accounts { get; set; } = [];
}

