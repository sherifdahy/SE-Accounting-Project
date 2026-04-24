using SA.Accounting.Core.Interfaces;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Windows;
using Telerik.Pivot.Core;

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

    public Task<bool> ShowDialogAsync(ViewModelBase viewModel)
    {
        var dialog = new DialogWindow
        {
            DataContext = viewModel,
            Owner = Application.Current.MainWindow
        };

        if (viewModel is ICloseable closeable)
        {
            closeable.CloseRequested += () =>
            {
                dialog.DialogResult = closeable.DialogResult;
                dialog.Close();
            };
        }

        var result = dialog.ShowDialog();

        return Task.FromResult(result == true);
    }
}
