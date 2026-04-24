using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.Core.WPF;

public interface IDialogService
{
    Task ShowInfoAsync(string message, string title);
    Task ShowWarningAsync(string message, string title);
    Task ShowErrorAsync(string message, string title);
    Task<bool> ShowConfirmAsync(string message, string title);
    Task<bool> ShowDialogAsync(ViewModelBase viewModel);
}
