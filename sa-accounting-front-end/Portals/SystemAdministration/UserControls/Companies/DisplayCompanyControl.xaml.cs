using SA.Accounting.WPF.Contracts.Account.Responses;
using SA.Accounting.WPF.ViewModels.Company;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;

public partial class DisplayCompanyControl : UserControl
{
    // ─── Constructor ─────────────────────────────
    public DisplayCompanyControl(DisplayCompanyViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    // ─── Load ────────────────────────────────────
    public void LoadCompany(int companyId)
    {
        if (DataContext is DisplayCompanyViewModel vm)
            vm.LoadCompanyCommand.Execute(companyId);
    }

    // ─── Account Card ────────────────────────────
    private void AccountCard_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: AccountResponse account }
            && DataContext is DisplayCompanyViewModel vm)
        {
            vm.OpenPlatformCommand.Execute(account);
        }
    }

    // ─── Copy ────────────────────────────────────
    private void CopyEmail_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string text } && !string.IsNullOrEmpty(text))
            Clipboard.SetText(text);
    }

    private void CopyPassword_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string text } && !string.IsNullOrEmpty(text))
            Clipboard.SetText(text);
    }

    // ─── Back ────────────────────────────────────
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.Parent is ContentControl parent)
        {
            // رجوع للقائمة الرئيسية
        }
    }
}