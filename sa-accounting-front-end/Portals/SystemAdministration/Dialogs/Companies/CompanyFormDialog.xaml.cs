using SA.Accounting.WPF.Clients.Company;
using SA.Accounting.WPF.Contracts.Company.Requests;
using System.Windows;

namespace SA.Accounting.WPF.Portals.SystemAdministration.Dialogs.Companies;

public partial class CompanyFormDialog : Window
{
    private readonly ICompanyClient _companyClient;

    public CompanyFormDialog(ICompanyClient companyClient)
    {
        InitializeComponent();
        DataContext = new CreateCompanyRequest();
        _companyClient = companyClient;
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var request = this.DataContext as CreateCompanyRequest;

        if (request != null)
        {
            await _companyClient.CreateAsync(request);
            DialogResult = true;
            Close();
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
