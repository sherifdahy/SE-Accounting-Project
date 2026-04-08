using SA.Accounting.Core.Interfaces;
using System.Windows;

namespace SA.Accounting.WPF.Services;

public sealed class DialogService : IDialogService
{
    public Task ShowInfoAsync(string message, string title)
    {
        MessageBox.Show(message, title,
            MessageBoxButton.OK, MessageBoxImage.Information);
        return Task.CompletedTask;
    }

    public Task ShowWarningAsync(string message, string title)
    {
        MessageBox.Show(message, title,
            MessageBoxButton.OK, MessageBoxImage.Warning);
        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(string message, string title)
    {
        MessageBox.Show(message, title,
            MessageBoxButton.OK, MessageBoxImage.Error);
        return Task.CompletedTask;
    }

    public Task<bool> ShowConfirmAsync(string message, string title)
    {
        var result = MessageBox.Show(message, title,
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        return Task.FromResult(result == MessageBoxResult.Yes);
    }
}
