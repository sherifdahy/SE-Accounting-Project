using SA.Accounting.Application.Contracts.Account.Requests;
using SA.Accounting.Application.Contracts.Owner.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Company.Requests;

public class UpdateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    public string TaxFileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<UpdateOwnerRequest> Owners { get; set; } = [];
    public List<UpdateAccountRequest> Accounts { get; set; } = [];
}
