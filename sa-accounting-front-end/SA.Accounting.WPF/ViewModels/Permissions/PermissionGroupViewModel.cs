using SA.Accounting.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SA.Accounting.WPF.ViewModels.Permissions;

// Models/PermissionGroupViewModel.cs
public class PermissionGroupViewModel : ViewModelBase
{
    public string GroupName { get; set; } = string.Empty;
    public ObservableCollection<PermissionItemViewModel> Permissions { get; set; } = [];

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set { _isExpanded = value; OnPropertyChanged(); }
    }

    public int TotalCount => Permissions.Count;
    public int SelectedCount => Permissions.Count(p => p.IsSelected);
    public string CountText => $"{SelectedCount}/{TotalCount}";

    // Progress 0.0 → 1.0
    public double Progress => TotalCount > 0 ? (double)SelectedCount / TotalCount : 0;

    public void Refresh()
    {
        OnPropertyChanged(nameof(SelectedCount));
        OnPropertyChanged(nameof(CountText));
        OnPropertyChanged(nameof(Progress));
    }
}
