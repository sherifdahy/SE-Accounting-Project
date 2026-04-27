using SA.Accounting.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.ViewModels.Permissions;

// Models/PermissionItemViewModel.cs
public class PermissionItemViewModel : ViewModelBase
{
    private readonly Action _onChanged;

    public PermissionItemViewModel(Action onChanged)
    {
        _onChanged = onChanged;
    }

    public string Permission { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string FullKey { get; set; } = string.Empty;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged();
            _onChanged?.Invoke();
        }
    }

    private bool _isDefault;
    public bool IsDefault
    {
        get => _isDefault;
        set { _isDefault = value; OnPropertyChanged(); }
    }
}
