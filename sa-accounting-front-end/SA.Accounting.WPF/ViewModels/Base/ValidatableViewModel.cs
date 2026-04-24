using FluentValidation.Results;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SA.Accounting.WPF.ViewModels.Base;

public abstract class ValidatableViewModel<T> : ViewModelBase, INotifyDataErrorInfo
{
    protected abstract IValidator<T> Validator { get; }

    private readonly Dictionary<string, List<string>> _errors = new();

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return _errors.SelectMany(x => x.Value);

        return _errors.TryGetValue(propertyName, out var errors)
            ? errors
            : Enumerable.Empty<string>();
    }

    public void ValidateProperty([CallerMemberName]string? propertyName = null)
    {
        var result = Validator.Validate((T)(object)this,
            o => o.IncludeProperties(propertyName));

        UpdateErrors(propertyName, result);
    }

    public void ValidateAll()
    {
        var result = Validator.Validate((T)(object)this);

        _errors.Clear();

        foreach (var error in result.Errors)
            AddError(error);

        NotifyAllErrors();
        OnPropertyChanged(nameof(HasErrors));
    }

    private void UpdateErrors(string propertyName, ValidationResult result)
    {
        _errors.Remove(propertyName);

        foreach (var error in result.Errors)
            AddError(error);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
    }

    private void AddError(ValidationFailure error)
    {
        if (!_errors.ContainsKey(error.PropertyName))
            _errors[error.PropertyName] = new List<string>();

        _errors[error.PropertyName].Add(error.ErrorMessage);
    }

    private void NotifyAllErrors()
    {
        foreach (var key in _errors.Keys)
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(key));
    }
}
