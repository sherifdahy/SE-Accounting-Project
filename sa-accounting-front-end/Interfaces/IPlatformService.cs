using SA.Accounting.WPF.Contracts.Common;
using SA.Accounting.WPF.Contracts.Company.Requests;
using SA.Accounting.WPF.Contracts.Company.Responses;
using SA.Accounting.WPF.Contracts.Platform.Requests;
using SA.Accounting.WPF.Contracts.Platform.Responses;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IPlatformService
{
    Task<List<PlatformResponse>> GetAllAsync(bool isDisabled = false);
    Task<PlatformDetailResponse> CreateAsync(PlatformRequest request);
    Task<PlatformDetailResponse> GetByIdAsync(int id);
    Task UpdateAsync(int id, PlatformRequest request);
    Task ToggleStatusAsync(int id);

}
