using CommunityToolkit.Mvvm.ComponentModel;
using FluentValidation;
using System.Collections;
using System.ComponentModel;

namespace SA.Accounting.WPF.Core;

public abstract class ValidatableModel<TRequest>
    : ObservableObject, INotifyDataErrorInfo
{
    private readonly IValidator<TRequest> _validator;
    private readonly Dictionary<string, List<string>> _errors = [];

    protected bool SuppressValidation { get; set; }

    protected ValidatableModel(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    // ─── INotifyDataErrorInfo ────────────────────
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _errors.Count != 0;
    public bool IsValid => !HasErrors;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
            return _errors.SelectMany(x => x.Value);

        return _errors.TryGetValue(propertyName, out var list)
            ? list
            : Enumerable.Empty<string>();
    }

    // ─── Validate property ───────────────────────
    protected void RunPropertyValidation(TRequest request, string propertyName)
    {
        if (SuppressValidation) return;

        _errors.Remove(propertyName);

        var result = _validator.Validate(request);

        var errors = result.Errors
            .Where(e => e.PropertyName == propertyName)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (errors.Count > 0)
            _errors[propertyName] = errors;

        RaiseErrorsChanged(propertyName);
        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(IsValid));
    }

    // ─── Validate all ────────────────────────────
    public void ValidateAll(TRequest request)
    {
        _errors.Clear();

        var result = _validator.Validate(request);

        foreach (var e in result.Errors)
        {
            if (!_errors.TryGetValue(e.PropertyName, out var list))
            {
                list = [];
                _errors[e.PropertyName] = list;
            }
            list.Add(e.ErrorMessage);
        }

        foreach (var key in _errors.Keys)
            RaiseErrorsChanged(key);

        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(IsValid));
    }

    // ─── Clear errors ────────────────────────────
    public void ClearAllErrors()
    {
        var keys = _errors.Keys.ToList();
        _errors.Clear();

        foreach (var key in keys)
            RaiseErrorsChanged(key);

        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(IsValid));
    }

    private void RaiseErrorsChanged(string propertyName)
        => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
}