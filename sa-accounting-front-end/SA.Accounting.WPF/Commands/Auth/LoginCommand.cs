using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels;
using System.Windows.Input;

namespace SA.Accounting.WPF.Commands.Auth;

public class LoginCommand : ICommand
{
    private readonly LoginViewModel _loginViewModel;
    private readonly IDialogService _dialogService;
    private readonly IAuthenticator _authenticator;
    private readonly IAppNavigationService _appNavigationService;

    public event EventHandler? CanExecuteChanged;
    public LoginCommand(
        LoginViewModel loginViewModel,
        IDialogService dialogService,
        IAuthenticator authenticator,
        IAppNavigationService appNavigationService)
    {
        _loginViewModel = loginViewModel;
        _dialogService = dialogService;
        _authenticator = authenticator;
        _appNavigationService = appNavigationService;
    }

    public bool CanExecute(object? parameter) => !_loginViewModel.HasErrors;

    public async void Execute(object? parameter)
    {
        try
        {
            await _authenticator.LoginAsync(_loginViewModel.Email, _loginViewModel.Password);
            _appNavigationService.LoginSuccess();
        }
        catch (ApiException ex)
        {
            var error = JsonConvert.DeserializeObject<ProblemDetails>(ex.Content!);
            _loginViewModel.ErrorMessage.Message = error!.Errors.First().Value.First();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Login Failed", ex.Message);
        }
    }
}