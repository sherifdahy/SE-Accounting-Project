using SA.Accounting.WPF.ViewModels.Company;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;

public partial class CreateCompanyControl : UserControl
{
    public CreateCompanyControl(CreateCompanyViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}