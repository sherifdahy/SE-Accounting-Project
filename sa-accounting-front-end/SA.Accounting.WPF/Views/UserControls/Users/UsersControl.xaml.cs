using SA.Accounting.Core.Contracts.User.Responses;
using SA.Accounting.WPF.ViewModels.User;
using SA.Accounting.Core.Interfaces;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace SA.Accounting.WPF.Views.UserControls.Users;

public partial class UsersControl : UserControl
{
    private readonly UsersViewModel _viewModel;
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;

    public UsersControl(
        UsersViewModel viewModel,
        IUserService userService,
        IDialogService dialogService)
    {
        _viewModel = viewModel;
        _userService = userService;
        _dialogService = dialogService;
        DataContext = viewModel;

        // ─── ربط حدث التعديل ───
        _viewModel.OnUpdateRequested += OnUpdateUser;

        InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadUsersAsync();
    }

    private void FilterComboBox_SelectionChanged(
        object sender, SelectionChangedEventArgs e)
    {
        if (_viewModel is null) return;

        if (sender is RadComboBox { SelectedItem: RadComboBoxItem item })
            _viewModel.FilterChangedCommand.Execute(item.Tag?.ToString());
    }

    // ─── إنشاء مستخدم ───────────────────────────
    private void AddUserButton_Click(object sender, RoutedEventArgs e)
    {
        var createVm = new CreateUserViewModel(
            _userService, _dialogService);

        createVm.OnSaved += async () =>
        {
            ShowList();
            await _viewModel.LoadUsersAsync();
        };

        createVm.OnCancelled += ShowList;

        ChildContent.Content = new CreateUserControl(createVm);
        ShowChild();
    }

    // ─── تعديل مستخدم ───────────────────────────
    private void OnUpdateUser(UserResponse user)
    {
        var updateVm = new UpdateUserViewModel(
            user.Id, _userService, _dialogService);

        updateVm.OnSaved += async () =>
        {
            ShowList();
            await _viewModel.LoadUsersAsync();
        };

        updateVm.OnCancelled += ShowList;

        ChildContent.Content = new UpdateUserControl(updateVm);
        ShowChild();
    }

    // ─── Navigation ─────────────────────────────
    private void ShowChild()
    {
        ListPanel.Visibility = Visibility.Collapsed;
        ChildContent.Visibility = Visibility.Visible;
    }

    private void ShowList()
    {
        ChildContent.Visibility = Visibility.Collapsed;
        ChildContent.Content = null;
        ListPanel.Visibility = Visibility.Visible;
    }
}