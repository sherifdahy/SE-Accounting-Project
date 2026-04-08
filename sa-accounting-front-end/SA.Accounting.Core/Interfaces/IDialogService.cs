namespace SA.Accounting.Core.Interfaces;

public interface IDialogService
{
    Task ShowInfoAsync(string message, string title);
    Task ShowWarningAsync(string message, string title);
    Task ShowErrorAsync(string message, string title);
    Task<bool> ShowConfirmAsync(string message, string title);
}
