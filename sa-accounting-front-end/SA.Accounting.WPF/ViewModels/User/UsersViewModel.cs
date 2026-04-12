using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.User.Responses;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Handlers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SA.Accounting.WPF.ViewModels.User;

public sealed partial class UsersViewModel : ObservableObject
{
    private readonly IUserService _userService;

    // ─── State ───────────────────────────────────
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _hasNoUsers;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _showDeleted;

    // ─── Statistics ──────────────────────────────
    [ObservableProperty] private int _totalUsers;
    [ObservableProperty] private int _totalActiveUsers;
    [ObservableProperty] private int _totalDeletedUsers;
    [ObservableProperty] private int _totalAdmins;
    [ObservableProperty] private string _paginationInfo = "عرض 0 من 0 مستخدم";

    // ─── Selected ────────────────────────────────
    [ObservableProperty] private UserResponse? _selectedUser;

    // ─── Events ──────────────────────────────────
    public event Action<UserResponse>? OnUpdateRequested;

    // ─── Collections ─────────────────────────────
    public ObservableCollection<UserResponse> AllUsers { get; } = [];
    public ObservableCollection<UserResponse> FilteredUsers { get; } = [];

    // ─── Constructor ─────────────────────────────
    public UsersViewModel(IUserService userService)
    {
        _userService = userService;
    }

    // ─── Property Change Hooks ───────────────────
    partial void OnSearchTextChanged(string value)
        => ApplySearchFilter();

    partial void OnShowDeletedChanged(bool value)
        => _ = LoadUsersAsync();

    // ─── Load Data (from Backend) ────────────────
    [RelayCommand]
    public async Task LoadUsersAsync()
    {
        IsBusy = true;
        try
        {
            await LoadStatisticsAsync();

            var filters = new RequestFilters();
            var result = await _userService.GetUsersAsync(filters, ShowDeleted);
            if (result?.Items is null) return;

            AllUsers.Clear();
            foreach (var u in result.Items)
                AllUsers.Add(u);

            ApplySearchFilter();
        }
        catch (Exception ex) { }
        finally { IsBusy = false; }
    }

    // ─── Statistics ──────────────────────────────
    private async Task LoadStatisticsAsync()
    {
        try
        {
            var filters = new RequestFilters();
            var all = await _userService.GetUsersAsync(filters, true);
            if (all?.Items is null) return;

            TotalUsers = all.Items.Count;
            //TotalActiveUsers = all.Items.Count(u => !u.IsDeleted);
            //TotalDeletedUsers = all.Items.Count(u => u.IsDeleted);
            TotalAdmins = all.Items.Count(u =>
                u.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false);
        }
        catch (Exception ex) {  }
    }

    // ─── Search Filter (local only) ─────────────
    private void ApplySearchFilter()
    {
        var filtered = AllUsers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLower();
            filtered = filtered.Where(u =>
                (u.Name?.ToLower().Contains(search) ?? false) ||
                (u.Email?.ToLower().Contains(search) ?? false) ||
                (u.PhoneNumber?.ToLower().Contains(search) ?? false) ||
                (u.Role?.ToLower().Contains(search) ?? false));
        }

        FilteredUsers.Clear();
        foreach (var u in filtered)
            FilteredUsers.Add(u);

        HasNoUsers = FilteredUsers.Count == 0;
        PaginationInfo = $"عرض {FilteredUsers.Count} من {AllUsers.Count} مستخدم";
    }

    // ─── Commands ────────────────────────────────

    [RelayCommand]
    private void UpdateUser(UserResponse? user)
    {
        if (user is null) return;
        OnUpdateRequested?.Invoke(user);
    }

    [RelayCommand]
    private async Task DeleteUserAsync(UserResponse? user)
    {
        if (user is null) return;
        try
        {
            var result = MessageBox.Show(
                $"هل أنت متأكد من حذف المستخدم \"{user.Name}\"؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            await _userService.ToggleStatusAsync(user.Id);
            await LoadUsersAsync();
        }
        catch (Exception ex) { }
    }

    [RelayCommand]
    private void FilterChanged(string? tag)
    {
        ShowDeleted = tag == "true";
    }
}