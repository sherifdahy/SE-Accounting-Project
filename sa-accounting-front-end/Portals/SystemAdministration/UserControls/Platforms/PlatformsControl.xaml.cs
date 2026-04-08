using SA.Accounting.WPF.Contracts.Platform.Responses;
using SA.Accounting.WPF.ViewModels.Platform;
using SA.Accounting.WPF.Interfaces;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;

public partial class PlatformsControl : UserControl
{
    private readonly PlatformsViewModel _viewModel;
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;

    public PlatformsControl(
        PlatformsViewModel viewModel,
        IPlatformService platformService,
        IDialogService dialogService)
    {
        _viewModel = viewModel;
        _platformService = platformService;
        _dialogService = dialogService;
        DataContext = viewModel;

        // ─── ربط حدث التعديل ───
        _viewModel.OnUpdateRequested += OnUpdatePlatform;

        InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadPlatformsAsync();
    }

    private void FilterComboBox_SelectionChanged(
        object sender, SelectionChangedEventArgs e)
    {
        if (_viewModel is null) return;

        if (sender is RadComboBox { SelectedItem: RadComboBoxItem item })
            _viewModel.FilterChangedCommand.Execute(item.Tag?.ToString());
    }

    // ─── إنشاء منصة ─────────────────────────────
    private void AddPlatformButton_Click(object sender, RoutedEventArgs e)
    {
        var createVm = new CreatePlatformViewModel(
            _platformService, _dialogService);

        createVm.OnSaved += async () =>
        {
            ShowList();
            await _viewModel.LoadPlatformsAsync();
        };

        createVm.OnCancelled += ShowList;

        ChildContent.Content = new CreatePlatformControl(createVm);
        ShowChild();
    }

    // ─── تعديل منصة ─────────────────────────────
    private void OnUpdatePlatform(PlatformResponse platform)
    {
        var updateVm = new UpdatePlatformViewModel(
            platform.Id, _platformService, _dialogService);

        updateVm.OnSaved += async () =>
        {
            ShowList();
            await _viewModel.LoadPlatformsAsync();
        };

        updateVm.OnCancelled += ShowList;

        ChildContent.Content = new UpdatePlatformControl(updateVm);
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