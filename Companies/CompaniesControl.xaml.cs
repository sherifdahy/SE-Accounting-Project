using Microsoft.Extensions.DependencyInjection;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;


namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;

public partial class CompaniesControl : UserControl
{
    private readonly System.IServiceProvider _serviceProvider;
    private readonly ICompanyService _companyService;

    public CompaniesControl(System.IServiceProvider serviceProvider,ICompanyService companyService)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _companyService = companyService;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await loadCompanies();
    }
    private async void CompaniesDataGrid_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e)
    {
        if (e.Row.DataContext is not CompanyResponse company)
            return;

        var control = _serviceProvider.GetRequiredService<DisplayCompanyControl>();

        this.Content = control;
        control.LoadCompany(company.Id);
    }

    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        //var confirm = DialogService.Confirm("سيتم مسح الشركة للأبد.", "تأكيد عمليه المسح");
        //if (!confirm)
            //return;

        if (sender is not Button { DataContext: CompanyResponse company })
            return;

        await _companyService.ToggleStatusAsync(company.Id);
        await loadCompanies();
    }

    private async void EditBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: CompanyResponse company })
            return;

        OpenUpdateControl(company.Id);
    }
    private async void CompaniesDataPager_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
    {
        //await UsePagination();
    }
    private async Task loadCompanies()
    {
        var paginationResult = await _companyService.GetAllAsync(CollectFilterData());

        CompaniesDataGrid.ItemsSource = paginationResult.Items;
        CompaniesDataPager.ItemCount = paginationResult.TotalCount;
    }
    private async void SearchBox_KeyUp(object sender, KeyEventArgs e)
    {
        await loadCompanies();
    }
    private async void ExportBtn_Click(object sender, RoutedEventArgs e)
    {
        //try
        //{
        //    var saveDialog = new SaveFileDialog()
        //    {
        //        Title = "اختر مكان حفظ الملف",
        //        Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
        //        FileName = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss} - Export File"
        //    };

        //    if (saveDialog.ShowDialog() != true)
        //        return;

        //    string filePath = saveDialog.FileName;
        //    var result = await _manager.DataSync.ExportToFileAsync(filePath);

        //    if (!result.State)
        //    {
        //        DialogService.ShowError(result.Message);
        //        return;
        //    }

        //    DialogService.ShowSuccess("تم حفظ البيانات في المجلد بنجاح.");
        //}
        //catch (Exception ex)
        //{
        //    DialogService.ShowError(ex.Message);
        //}
    }
    private async void ImportBtn_Click(object sender, RoutedEventArgs e)
    {
        //try
        //{
        //    var fileDialog = new OpenFileDialog()
        //    {
        //        Title = "اختر مكان الملف",
        //        Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
        //    };

        //    if (fileDialog.ShowDialog() != true)
        //        return;

        //    string filePath = fileDialog.FileName;
        //    var result = await _manager.DataSync.ImportFromFileAsync(filePath);

        //    if (!result.State)
        //    {
        //        DialogService.ShowError(result.Message);
        //        return;
        //    }

        //    DialogService.ShowSuccess($"Importing file {filePath}");
        //}
        //catch (Exception ex)
        //{
        //    DialogService.ShowError(ex.Message);
        //}

    }
    private async void AddCompanyButton_Click(object sender, RoutedEventArgs e)
    {
        var control = _serviceProvider.GetRequiredService<CreateCompanyControl>();

        this.Content = control;
    }
    private async void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!FilterComboBox.IsLoaded)
            return;

        await loadCompanies();
    }
    private RequestFilters CollectFilterData()
    {
        var selectedItem = FilterComboBox.SelectedItem as RadComboBoxItem;
        bool showAll = false;
        if (selectedItem != null)
        {
            bool.TryParse(selectedItem.Tag?.ToString(), out showAll);
        }

        var searchValue = SearchBox.Text;

        return new RequestFilters()
        {
            SearchValue = searchValue,
            PageSize = CompaniesDataPager.PageSize,
            PageNumber = CompaniesDataPager.PageIndex + 1,
            SortColumn = "Id",
            SortDirection = "DESC"
        };
    }
    private void OpenUpdateControl(int companyId)
    {
        var control = _serviceProvider.GetRequiredService<UpdateCompanyControl>();

        this.Content = control;

        control.LoadCompany(companyId);
    }
}
