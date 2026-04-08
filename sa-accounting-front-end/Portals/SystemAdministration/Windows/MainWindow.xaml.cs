using Microsoft.Extensions.DependencyInjection;
using SA.Accounting.WPF.Abstraction;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Employees;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;
using SA.Accounting.WPF.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SA.Accounting.WPF.Portals.SystemAdministration.Windows;

public partial class MainWindow : Window
{
    private readonly System.IServiceProvider _serviceProvider;
    private readonly IPermissionService _permissionService;

    public MainWindow(System.IServiceProvider serviceProvider, IPermissionService permissionService)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _permissionService = permissionService;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await LoadUserInfoAsync();
        await ApplyPermissionsAsync();
        HomePageBtn_Click(HomePageBtn, e);
    }

    // =========================
    // User Info
    // =========================
    private async Task LoadUserInfoAsync()
    {
        var userEmail = await _permissionService.GetUserEmailAsync();

        UserEmailTxt.Text = userEmail;
        MenuUserEmail.Text = userEmail;

        // TODO: Load role name from service
        UserRoleTxt.Text = "مدير النظام";
        MenuUserRole.Text = "مدير النظام";
    }

    // =========================
    // Apply Permissions
    // =========================
    private async Task ApplyPermissionsAsync()
    {
        // الشركات
        bool canViewCompanies = await _permissionService.HasPermissionAsync("companies:read");
        CompanyPageBtn.Visibility = canViewCompanies ? Visibility.Visible : Visibility.Collapsed;

        // المنظمات
        bool canViewPlatfroms = await _permissionService.HasPermissionAsync("platforms:read");
        PlatfromsBtn.Visibility = canViewPlatfroms ? Visibility.Visible : Visibility.Collapsed;

        // الموظفين
        bool canViewEmployees = await _permissionService.HasPermissionAsync("employees:read");
        EmployeesBtn.Visibility = canViewEmployees ? Visibility.Visible : Visibility.Collapsed;

        // العهد
        bool canViewTransactions = await _permissionService.HasPermissionAsync("transactions:read");
        TransactionsBtn.Visibility = canViewTransactions ? Visibility.Visible : Visibility.Collapsed;

        // الإعدادات
        bool canViewSettings = await _permissionService.HasPermissionAsync("roles:read");
        SettingsBtn.Visibility = canViewSettings ? Visibility.Visible : Visibility.Collapsed;

        // التقارير (مفتوحة للجميع)
        ReportsBtn.Visibility = Visibility.Visible;
    }

    // =========================
    // Event Handlers
    // =========================
    private void HomePageBtn_Click(object sender, RoutedEventArgs e)
    {
        SetActiveButton((Button)sender);
        PageTitleHeader.Text = "لوحة التحكم";
        MainSection.Content = null; // TODO: Add home page content
    }

    private void CompanyPageBtn_Click(object sender, RoutedEventArgs e)
    {
        SetActiveButton((Button)sender);
        PageTitleHeader.Text = "إدارة الشركات";

        var companiesControl = _serviceProvider.GetRequiredService<CompaniesControl>();
        MainSection.Content = companiesControl;
    }

    private void PlatfromsBtn_Click(object sender, RoutedEventArgs e)
    {
        SetActiveButton((Button)sender);
        PageTitleHeader.Text = "إدارة المنظمات";

        var platformsControl = _serviceProvider.GetRequiredService<PlatformsControl>();
        MainSection.Content = platformsControl;
    }

    private void EmployeesBtn_Click(object sender, RoutedEventArgs e)
    {
        SetActiveButton((Button)sender);
        PageTitleHeader.Text = "إدارة الموظفين";

        var employeesControl = _serviceProvider.GetRequiredService<EmployeesControl>();
        MainSection.Content = employeesControl;
    }

    private void TransactionsBtn_Click(object sender, RoutedEventArgs e)
    {
        SetActiveButton((Button)sender);
        PageTitleHeader.Text = "إدارة العهد";
        MainSection.Content = null; // TODO: Add transactions content
    }

    private void SettingsBtn_Click(object sender, RoutedEventArgs e)
    {
        SetActiveButton((Button)sender);
        PageTitleHeader.Text = "الإعدادات";
        MainSection.Content = null; // TODO: Add settings content
    }

    // =========================
    // UI Helpers
    // =========================
    private void SetActiveButton(Button activeBtn)
    {
        // Clear all tags
        foreach (var child in SidebarContent.Children)
        {
            if (child is Button btn)
                btn.Tag = null;
        }

        // Set active
        activeBtn.Tag = "Selected";
    }

    private void UserMenuBtn_Click(object sender, RoutedEventArgs e)
    {
        UserMenuPopup.IsOpen = !UserMenuPopup.IsOpen;
    }

    private void UserInfoBottom_Click(object sender, MouseButtonEventArgs e)
    {
        UserMenuPopup.PlacementTarget = UserMenuBtn;
        UserMenuPopup.IsOpen = true;
    }

    private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
    {
        UserMenuPopup.IsOpen = false;
        MessageBox.Show("الملف الشخصي", "معلومات", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async void LogoutBtn_Click(object sender, RoutedEventArgs e)
    {
        UserMenuPopup.IsOpen = false;

        var result = MessageBox.Show(
            "هل أنت متأكد من تسجيل الخروج؟",
            "تأكيد تسجيل الخروج",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        await cacheService.RemoveAsync(KeysConstant.AuthResponse);

        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var loginWindow = App.Services.GetRequiredService<LoginWindow>();

        this.Close();

        var loginResult = loginWindow.ShowDialog();

        if (loginResult == true)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var mainWindow = App.Services.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
        else
        {
            Application.Current.Shutdown();
        }
    }
}