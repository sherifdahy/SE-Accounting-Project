using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SA.Accounting.Core.Contracts.Platform.Responses;
using SA.Accounting.Infrastructure.Handlers;
using SA.Accounting.Core.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SA.Accounting.WPF.ViewModels.Platform;

public sealed partial class PlatformsViewModel : ObservableObject
{
    private readonly IPlatformService _platformService;

    // ─── State ───────────────────────────────────
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _hasNoPlatforms;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _showDeleted;

    // ─── Statistics ──────────────────────────────
    [ObservableProperty] private int _totalPlatforms;
    [ObservableProperty] private int _totalActivePlatforms;
    [ObservableProperty] private int _totalDeletedPlatforms;
    [ObservableProperty] private int _totalUrls;
    [ObservableProperty] private string _paginationInfo = "عرض 0 من 0 منصة";

    // ─── Selected ────────────────────────────────
    [ObservableProperty] private PlatformResponse? _selectedPlatform;

    // ─── Events ──────────────────────────────────
    public event Action<PlatformResponse>? OnUpdateRequested;

    // ─── Collections ─────────────────────────────
    public ObservableCollection<PlatformResponse> AllPlatforms { get; } = [];
    public ObservableCollection<PlatformResponse> FilteredPlatforms { get; } = [];

    // ─── Constructor ─────────────────────────────
    public PlatformsViewModel(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    // ─── Property Change Hooks ───────────────────
    partial void OnSearchTextChanged(string value)
        => ApplySearchFilter();

    partial void OnShowDeletedChanged(bool value)
        => _ = LoadPlatformsAsync();

    // ─── Load Data (from Backend) ────────────────
    [RelayCommand]
    public async Task LoadPlatformsAsync()
    {
        IsBusy = true;
        try
        {
            await LoadStatisticsAsync();

            var platforms = await _platformService.GetAllAsync(ShowDeleted);
            if (platforms is null) return;

            AllPlatforms.Clear();
            foreach (var p in platforms)
                AllPlatforms.Add(p);

            ApplySearchFilter();
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
        finally { IsBusy = false; }
    }

    // ─── Statistics ──────────────────────────────
    private async Task LoadStatisticsAsync()
    {
        try
        {
            var all = await _platformService.GetAllAsync(true);
            if (all is null) return;

            TotalPlatforms = all.Count;
            TotalActivePlatforms = all.Count(p => !p.IsDeleted);
            TotalDeletedPlatforms = all.Count(p => p.IsDeleted);
            TotalUrls = all.Count(p => !string.IsNullOrWhiteSpace(p.Url));
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
    }

    // ─── Search Filter (local only) ─────────────
    private void ApplySearchFilter()
    {
        var filtered = AllPlatforms.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLower();
            filtered = filtered.Where(p =>
                (p.Name?.ToLower().Contains(search) ?? false) ||
                (p.Url?.ToLower().Contains(search) ?? false));
        }

        FilteredPlatforms.Clear();
        foreach (var p in filtered)
            FilteredPlatforms.Add(p);

        HasNoPlatforms = FilteredPlatforms.Count == 0;
        PaginationInfo = $"عرض {FilteredPlatforms.Count} من {AllPlatforms.Count} منصة";
    }

    // ─── Commands ────────────────────────────────

    [RelayCommand]
    private void UpdatePlatform(PlatformResponse? platform)
    {
        if (platform is null) return;
        OnUpdateRequested?.Invoke(platform);
    }

    [RelayCommand]
    private async Task DeletePlatformAsync(PlatformResponse? platform)
    {
        if (platform is null) return;
        try
        {
            var result = MessageBox.Show(
                $"هل أنت متأكد من حذف منصة \"{platform.Name}\"؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            await _platformService.ToggleStatusAsync(platform.Id);
            await LoadPlatformsAsync();
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
    }

    [RelayCommand]
    private void OpenUrl(PlatformResponse? platform)
    {
        if (string.IsNullOrWhiteSpace(platform?.Url)) return;
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = platform.Url,
                UseShellExecute = true
            });
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
    }

    [RelayCommand]
    private void FilterChanged(string? tag)
    {
        ShowDeleted = tag == "true";
    }
}