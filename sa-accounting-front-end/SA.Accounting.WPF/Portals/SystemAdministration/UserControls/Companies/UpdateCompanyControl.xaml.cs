using SA.Accounting.WPF.ViewModels.Company;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;

public partial class UpdateCompanyControl : UserControl
{
    public UpdateCompanyControl(UpdateCompanyViewModel updateCompanyView)
    {
        InitializeComponent();
        DataContext = updateCompanyView;
    }

    public async Task LoadCompany(int companyId)
    {
        if (DataContext is UpdateCompanyViewModel vm)
            await vm.LoadCompanyAsync(companyId);
    }

}