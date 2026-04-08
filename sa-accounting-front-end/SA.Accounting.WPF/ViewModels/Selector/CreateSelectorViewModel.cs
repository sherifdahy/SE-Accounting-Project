using CommunityToolkit.Mvvm.ComponentModel;
using SA.Accounting.Core.Contracts.Platform.Requests;
using SA.Accounting.Core.Contracts.Platform.Validators;
using SA.Accounting.Core.Contracts.Selector.Requests;
using SA.Accounting.Core.Contracts.Selector.Validators;
using SA.Accounting.Core;
using SA.Accounting.Core.Enums;

namespace SA.Accounting.WPF.ViewModels.Selector;

public sealed partial class CreateSelectorViewModel
    : ValidatableModel<SelectorRequest>
{
    [ObservableProperty] private string _value = string.Empty;
    [ObservableProperty] private SelectorContentType _contentType;
    [ObservableProperty] private SelectorType _type;
    [ObservableProperty] private int _priority;

    // ─── Display property (read-only, يتحدث من الأب) ───
    [ObservableProperty] private int _displayOrder;

    public CreateSelectorViewModel() : base(new SelectorRequestValidator())
    {
    }

    partial void OnValueChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Value));

    partial void OnContentTypeChanged(SelectorContentType value)
        => RunPropertyValidation(ToRequest(), nameof(ContentType));

    partial void OnTypeChanged(SelectorType value)
        => RunPropertyValidation(ToRequest(), nameof(Type));

    public SelectorRequest ToRequest() => new()
    {
        Value = Value,
        ContentType = ContentType,
        Type = Type,
        Priority = Priority
    };
}